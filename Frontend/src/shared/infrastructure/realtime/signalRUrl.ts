import { API_ORIGIN } from '@/shared/infrastructure/http/apiUrl';

const BACKEND_ORIGIN = API_ORIGIN;

export function getSignalRHubUrl(hubPath: string): string {
  const origin = BACKEND_ORIGIN.replace(/\/+$/, '');
  const path = hubPath.startsWith('/') ? hubPath : `/${hubPath}`;
  return `${origin}${path}`;
}
