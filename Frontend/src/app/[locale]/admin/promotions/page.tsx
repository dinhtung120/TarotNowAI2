import dynamic from 'next/dynamic';
import AdminRouteLoadingFallback from '@/shared/components/loading/AdminRouteLoadingFallback';

const AdminPromotionsPage = dynamic(
 () => import('@/features/admin/public').then((m) => m.AdminPromotionsPage),
 {
  loading: () => <AdminRouteLoadingFallback />,
 }
);

export default function AdminPromotionsRoutePage() {
 return <AdminPromotionsPage />;
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';
