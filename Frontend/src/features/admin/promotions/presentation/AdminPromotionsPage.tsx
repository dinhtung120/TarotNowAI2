import {
  listPromotions as listPromotionsAction,
  type DepositPromotion,
} from '@/features/admin/application/actions/promotion';
import AdminPromotionsClient from '@/features/admin/promotions/presentation/AdminPromotionsClient';

export default async function AdminPromotionsPage() {
  const promotionsResult = await listPromotionsAction(false);
  const initialPromotions: DepositPromotion[] =
    promotionsResult.success && promotionsResult.data ? promotionsResult.data : [];

  return <AdminPromotionsClient initialPromotions={initialPromotions} />;
}
