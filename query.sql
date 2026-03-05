INSERT INTO ida.cliente_modelo_controle ("IdCliente", "IdModeloControle", "DtUltimaAtualizacao") VALUES ((SELECT "Id" FROM ida.cliente WHERE "Cnpj" = '11111111000111'), 1, CURRENT_TIMESTAMP);
