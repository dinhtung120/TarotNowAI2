import {
 listPromotions as listPromotionsAction,
 type DepositPromotion,
} from "@/actions/promotionActions";
import AdminPromotionsClient from "./promotions-client";

export default async function AdminPromotionsPage() {
 const initialPromotions: DepositPromotion[] = (await listPromotionsAction(false)) ?? [];

 return <AdminPromotionsClient initialPromotions={initialPromotions} />;
}
