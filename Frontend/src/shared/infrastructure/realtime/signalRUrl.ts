import { getApiOrigin } from '@/shared/infrastructure/http/apiUrl';



export function getSignalRHubUrl(hubPath: string): string {
  const origin = getApiOrigin().replace(/\/+$/, '');
  const path = hubPath.startsWith('/') ? hubPath : `/${hubPath}`;
  return `${origin}${path}`;
}
