

const DEFAULT_BACKEND_ORIGIN = 'http://127.0.0.1:5037';

function resolveBackendOrigin(): string {
  const raw = process.env.NEXT_PUBLIC_API_URL;

  if (!raw || raw.trim().length === 0) {
    return DEFAULT_BACKEND_ORIGIN;
  }

  try {
    const url = new URL(raw.trim());
    return url.origin; // Ví dụ: 'http://localhost:5037'
  } catch {
    return DEFAULT_BACKEND_ORIGIN;
  }
}

const BACKEND_ORIGIN = resolveBackendOrigin();

export function getSignalRHubUrl(hubPath: string): string {
  const origin = BACKEND_ORIGIN.replace(/\/+$/, '');
  const path = hubPath.startsWith('/') ? hubPath : `/${hubPath}`;
  return `${origin}${path}`;
}
