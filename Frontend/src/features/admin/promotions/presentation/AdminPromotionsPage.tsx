import {
  listPromotions as listPromotionsAction,
  type DepositPromotion,
} from '@/features/admin/application/actions/promotion';
import AdminPromotionsClient from '@/features/admin/promotions/presentation/AdminPromotionsClient';

export default async function AdminPromotionsPage() {
  const promotionsResult = await listPromotionsAction(false);
  const initialPromotions: DepositPromotion[] = promotionsResult.success
   ? promotionsResult.data ?? []
   : [];
  const initialLoadError = promotionsResult.success
   ? null
   : (promotionsResult.error || 'Failed to list promotions');

  return <AdminPromotionsClient initialPromotions={initialPromotions} initialLoadError={initialLoadError} />;
}
