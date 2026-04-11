import dynamic from 'next/dynamic';

const AdminPromotionsPage = dynamic(
 () => import('@/features/admin/promotions/presentation/AdminPromotionsPage').then((m) => m.default),
 {
  loading: () => (
   <div className="flex min-h-[35vh] items-center justify-center text-sm text-slate-400">Loading…</div>
  ),
 }
);

export default function AdminPromotionsRoutePage() {
 return <AdminPromotionsPage />;
}
