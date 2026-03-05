-- SQL Script para popular a base de dados do sistema IDA com uma massa MASSIVA de dados
-- Senha 'T4k3d4@@!': $2a$11$iN0RlRFQOK.eJCMVeMf4muOB33fWVR0HYd3Nb5XzdSKwqBnLZsmRC

START TRANSACTION;

-- 1. Bases: Áreas e Cargos
INSERT INTO ida.area ("Nome", "DtUltimaAtualizacao") VALUES
('Vendas Diretas', CURRENT_TIMESTAMP),
('Marketing Digital', CURRENT_TIMESTAMP),
('Recepção e Atendimento', CURRENT_TIMESTAMP),
('Gestão Comercial', CURRENT_TIMESTAMP),
('Performance', CURRENT_TIMESTAMP),
('Inside Sales', CURRENT_TIMESTAMP),
('Field Sales', CURRENT_TIMESTAMP),
('Vendas Externas', CURRENT_TIMESTAMP),
('Sucesso do Cliente', CURRENT_TIMESTAMP),
('Logística e Operações', CURRENT_TIMESTAMP),
('Financeiro Corp', CURRENT_TIMESTAMP)
ON CONFLICT DO NOTHING;

INSERT INTO ida.cargo ("Nome", "IdRole", "DtUltimaAtualizacao") VALUES
('Diretor Executivo', 2, CURRENT_TIMESTAMP),
('Supervisor de Vendas', 3, CURRENT_TIMESTAMP),
('Coordenador de Marketing', 3, CURRENT_TIMESTAMP),
('Consultor de Vendas', 4, CURRENT_TIMESTAMP),
('Atendente Especializado', 4, CURRENT_TIMESTAMP),
('Gestor de Tráfego', 4, CURRENT_TIMESTAMP),
('Consultor Inside Sales', 4, CURRENT_TIMESTAMP),
('Consultor Field Sales', 4, CURRENT_TIMESTAMP),
('Gerente Regional', 3, CURRENT_TIMESTAMP),
('Analista de CS Senior', 4, CURRENT_TIMESTAMP),
('Account Manager', 4, CURRENT_TIMESTAMP)
ON CONFLICT DO NOTHING;

-- 2. Cargo_Area (Vínculos essenciais)
INSERT INTO ida.cargo_area ("IdCargo", "IdArea", "DtUltimaAtualizacao")
SELECT cg."Id", a."Id", CURRENT_TIMESTAMP
FROM ida.cargo cg, ida.area a
WHERE (cg."Nome" = 'Diretor Executivo' AND a."Nome" = 'Gestão Comercial')
   OR (cg."Nome" = 'Supervisor de Vendas' AND a."Nome" = 'Vendas Diretas')
   OR (cg."Nome" = 'Consultor de Vendas' AND a."Nome" = 'Vendas Diretas')
   OR (cg."Nome" = 'Consultor Inside Sales' AND a."Nome" = 'Inside Sales')
   OR (cg."Nome" = 'Consultor Field Sales' AND a."Nome" = 'Field Sales')
ON CONFLICT DO NOTHING;

-- 3. Inserir Clientes Primários (Tech Corp etc)
INSERT INTO ida.cliente ("Nome", "Cnpj", "Email", "Telefone", "DtUltimaAtualizacao") VALUES
('Tech Corp S/A', '66666666000166', 'corp@techcorp.com', '11999999996', CURRENT_TIMESTAMP),
('Mega Varejo Ltda', '33333333000133', 'gerencia@megavarejo.com.br', '11999999993', CURRENT_TIMESTAMP)
ON CONFLICT DO NOTHING;

-- Modelos de Controle para os primários
INSERT INTO ida.cliente_modelo_controle ("IdCliente", "IdModeloControle", "DtUltimaAtualizacao") VALUES 
((SELECT "Id" FROM ida.cliente WHERE "Cnpj" = '66666666000166'), 1, CURRENT_TIMESTAMP),
((SELECT "Id" FROM ida.cliente WHERE "Cnpj" = '33333333000133'), 2, CURRENT_TIMESTAMP)
ON CONFLICT DO NOTHING;

-- 4. Bloco de Geração dinâmica de 20 Clientes e seus Usuários
DO $$
DECLARE
    i INT;
    c_id BIGINT;
    p_id BIGINT;
    a_id BIGINT;
    cg_id BIGINT;
    v_idx INT;
BEGIN
    RAISE NOTICE 'Iniciando geração de clientes e usuários...';
    FOR i IN 1..20 LOOP
        -- Cliente
        INSERT INTO ida.cliente ("Nome", "Cnpj", "Email", "Telefone", "DtUltimaAtualizacao")
        VALUES ('Empresa de Teste ' || i, LPAD((200+i)::text, 14, '0'), 'contato' || i || '@test.com', '119', CURRENT_TIMESTAMP)
        RETURNING "Id" INTO c_id;

        -- Modelo (Aleatório)
        INSERT INTO ida.cliente_modelo_controle ("IdCliente", "IdModeloControle", "DtUltimaAtualizacao")
        VALUES (c_id, (floor(random() * 3) + 1), CURRENT_TIMESTAMP);

        -- Todos os cargos para esse cliente
        INSERT INTO ida.cliente_cargo ("IdCliente", "IdCargo", "DtUltimaAtualizacao")
        SELECT c_id, "Id", CURRENT_TIMESTAMP FROM ida.cargo;

        -- Criar 1 Proprietário e 2 Vendedores
        -- Proprietário
        INSERT INTO ida.pessoa ("Nome", "Cpf", "Email", "Telefone", "IdCliente", "DtUltimaAtualizacao")
        VALUES ('Prop. ' || i, LPAD((i*1000)::text, 11, '0'), 'p' || i || '@t.com', '11', c_id, CURRENT_TIMESTAMP)
        RETURNING "Id" INTO p_id;
        
        cg_id := (SELECT "Id" FROM ida.cargo WHERE "Nome" = 'Diretor Executivo' LIMIT 1);
        INSERT INTO ida.usuario ("Id", "Login", "Senha", "FlAtivo", "IdCargo", "DtUltimaAtualizacao")
        VALUES (p_id, 'prop.' || i, '$2a$11$iN0RlRFQOK.eJCMVeMf4muOB33fWVR0HYd3Nb5XzdSKwqBnLZsmRC', true, cg_id, CURRENT_TIMESTAMP);
        
        a_id := (SELECT "Id" FROM ida.area WHERE "Nome" = 'Gestão Comercial' LIMIT 1);
        INSERT INTO ida.usuario_area ("IdUsuario", "IdArea", "DtUltimaAtualizacao") VALUES (p_id, a_id, CURRENT_TIMESTAMP);
        INSERT INTO ida.cliente_usuario ("IdCliente", "IdUsuario", "IdArea", "DtUltimaAtualizacao") VALUES (c_id, p_id, a_id, CURRENT_TIMESTAMP);

        -- 2 Vendedores
        FOR v_idx IN 1..2 LOOP
            INSERT INTO ida.pessoa ("Nome", "Cpf", "Email", "Telefone", "IdCliente", "DtUltimaAtualizacao")
            VALUES ('Vend ' || i || '.' || v_idx, LPAD((i*1000+v_idx)::text, 11, '1'), 'v' || i || '.' || v_idx || '@t.com', '11', c_id, CURRENT_TIMESTAMP)
            RETURNING "Id" INTO p_id;
            
            cg_id := (SELECT "Id" FROM ida.cargo WHERE "Nome" = 'Consultor de Vendas' LIMIT 1);
            INSERT INTO ida.usuario ("Id", "Login", "Senha", "FlAtivo", "IdCargo", "DtUltimaAtualizacao")
            VALUES (p_id, 'user.' || i || '.' || v_idx, '$2a$11$iN0RlRFQOK.eJCMVeMf4muOB33fWVR0HYd3Nb5XzdSKwqBnLZsmRC', true, cg_id, CURRENT_TIMESTAMP);
            
            a_id := (SELECT "Id" FROM ida.area ORDER BY random() LIMIT 1);
            INSERT INTO ida.usuario_area ("IdUsuario", "IdArea", "DtUltimaAtualizacao") VALUES (p_id, a_id, CURRENT_TIMESTAMP);
            INSERT INTO ida.cliente_usuario ("IdCliente", "IdUsuario", "IdArea", "DtUltimaAtualizacao") VALUES (c_id, p_id, a_id, CURRENT_TIMESTAMP);
        END LOOP;
    END LOOP;
    RAISE NOTICE 'Clientes e usuários gerados com sucesso.';
END $$;

-- 5. Lançamentos Massivos (Ajustado para ser mais performático e seguro)
DO $$
DECLARE
    u_rec RECORD;
    d_lanc DATE;
    l_id BIGINT;
    m_id INT;
BEGIN
    RAISE NOTICE 'Iniciando geração de lançamentos históricos...';
    FOR u_rec IN (
        SELECT u."Id", cmc."IdModeloControle"
        FROM ida.usuario u
        JOIN ida.pessoa p ON u."Id" = p."Id"
        JOIN ida.cliente_modelo_controle cmc ON p."IdCliente" = cmc."IdCliente"
        WHERE u."IdCargo" IN (SELECT "Id" FROM ida.cargo WHERE "Nome" LIKE 'Consultor%')
    ) LOOP
        -- Lançamentos semanais nos últimos 2 anos
        FOR d_lanc IN (SELECT generate_series(CURRENT_DATE - INTERVAL '2 years', CURRENT_DATE, '7 days')::date) LOOP
            INSERT INTO ida.lancamento (id_usuario, id_modelo_controle, data_lancamento, "DtUltimaAtualizacao")
            VALUES (u_rec."Id", u_rec."IdModeloControle", d_lanc, CURRENT_TIMESTAMP)
            RETURNING "Id" INTO l_id;

            IF u_rec."IdModeloControle" = 1 THEN
                INSERT INTO ida.lancamento_cadastro ("Id", qtd_click_link, qtd_cadastros, vlr_ticket_medio, vlr_investimento_meta, vlr_investimento_google)
                VALUES (l_id, floor(random()*500), floor(random()*50), random()*200, random()*1000, random()*1000);
            ELSIF u_rec."IdModeloControle" = 2 THEN
                INSERT INTO ida.lancamento_varejo ("Id", qtd_atendimento, qtd_fechamento, faturamento, qtd_instagram, qtd_facebook, qtd_google, qtd_indicacao, vlr_investimento_meta, vlr_investimento_google)
                VALUES (l_id, floor(random()*100), floor(random()*20), random()*50000, floor(random()*50), floor(random()*50), floor(random()*50), floor(random()*10), random()*2000, random()*2500);
            ELSIF u_rec."IdModeloControle" = 3 THEN
                INSERT INTO ida.lancamento_saude ("Id", qtd_click_meta, qtd_click_google, qtd_contatos_reais, qtd_conversao_consultas, vlr_ticket_medio_consultas, qtd_entrada_redes_sociais, qtd_entrada_google, vlr_investimento_meta, vlr_investimento_google)
                VALUES (l_id, floor(random()*200), floor(random()*200), floor(random()*50), floor(random()*15), random()*500, floor(random()*30), floor(random()*30), random()*1500, random()*1500);
            END IF;
        END LOOP;
    END LOOP;
    RAISE NOTICE 'Lançamentos gerados com sucesso.';
END $$;

COMMIT;
