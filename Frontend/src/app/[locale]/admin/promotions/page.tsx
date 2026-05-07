import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/app/_shared/app-shell/loading/AdminRouteLoadingFallback';

const AdminPromotionsPage = dynamic(
 () => import('@/features/admin/promotions/AdminPromotionsPage'),
 {
  loading: () => <AdminRouteLoadingFallback />,
 }
);

export default function AdminPromotionsRoutePage() {
 return <AdminPromotionsPage />;
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';
