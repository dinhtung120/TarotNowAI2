export function getSignalRHubUrl(hubPath: string): string {
  const path = hubPath.startsWith('/') ? hubPath : `/${hubPath}`;
  return path;
}
