import axios from 'axios';

const BASE_URL = 'http://localhost:5100/api';

const api = axios.create({
  baseURL: BASE_URL,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Interceptor de resposta: captura erros HTTP e grava no log do backend
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Não logar erros de autenticação (401) para evitar loops
    if (error?.response?.status !== 401) {
      const logPayload = {
        message: `[Axios Error] ${error?.response?.status ?? 'Network Error'}: ${error?.message}`,
        stack: error?.stack ?? '',
        url: error?.config?.url ?? window.location.href,
        additionalData: JSON.stringify({
          method: error?.config?.method,
          requestData: error?.config?.data,
          responseData: error?.response?.data,
        }),
        user: localStorage.getItem('user') ?? 'Desconhecido',
      };

      // Envio silencioso, sem bloquear o fluxo normal
      fetch(`${BASE_URL}/log/frontend`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(logPayload),
      }).catch(() => {
        // Ignorar falha do log para não causar erro em cascata
      });
    }

    return Promise.reject(error);
  }
);

export default api;
