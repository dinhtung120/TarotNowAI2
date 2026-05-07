import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/app/_shared/app-shell/loading/AdminRouteLoadingFallback';
import { getServerAccessToken } from '@/shared/auth/serverAuth';
import { serverHttpRequest } from '@/shared/http/serverHttpClient';
import type { AdminSystemConfigItem } from '@/features/admin/system-configs/system-config.types';

const AdminSystemConfigsPage = dynamic(
 () => import('@/features/admin/system-configs/AdminSystemConfigsPage'),
 {
  loading: () => <AdminRouteLoadingFallback />,
 },
);

export async function generateMetadata() {
 return {
  title: 'System Configs - TarotNow Admin',
 };
}

export default async function AdminSystemConfigsRoutePage() {
 const accessToken = await getServerAccessToken();
 if (!accessToken) {
  return <AdminSystemConfigsPage initialConfigs={[]} />;
 }

 const response = await serverHttpRequest<AdminSystemConfigItem[]>('/admin/system-configs', {
  method: 'GET',
  token: accessToken,
  next: { revalidate: 0 },
 });

 const initialConfigs = response.ok ? response.data ?? [] : [];
 return <AdminSystemConfigsPage initialConfigs={initialConfigs} />;
}
