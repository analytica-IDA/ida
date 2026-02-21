import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App'

const LOG_URL = 'http://localhost:5100/api/log/frontend';

function sendErrorLog(message: string, stack: string, additionalData?: string) {
  fetch(LOG_URL, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      message,
      stack,
      url: window.location.href,
      additionalData: additionalData ?? '',
      user: localStorage.getItem('user') ?? 'Desconhecido',
    }),
  }).catch(() => {
    // Silencioso: falha no log não deve quebrar o app
  });
}

// Captura erros de runtime JavaScript (ex: TypeError, ReferenceError)
window.addEventListener('error', (event) => {
  sendErrorLog(
    `[JS Error] ${event.message}`,
    event.error?.stack ?? `${event.filename}:${event.lineno}:${event.colno}`
  );
});

// Captura Promises rejeitadas sem tratamento (ex: await sem try/catch)
window.addEventListener('unhandledrejection', (event) => {
  const reason = event.reason;
  sendErrorLog(
    `[Unhandled Promise] ${reason?.message ?? String(reason)}`,
    reason?.stack ?? ''
  );
});

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
