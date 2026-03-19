/*
 * ===================================================================
 * FILE: page.tsx (Admin Promotions)
 * BỐI CẢNH (CONTEXT):
 *   Trang gốc quản lý Khuyến mãi (Promotions).
 * 
 * RENDERING (SERVER COMPONENT):
 *   Đây là component chạy trên Server. Nó có nhiệm vụ lấy sẵn dữ liệu listPromotionsAction
 *   trước khi gửi HTML xuống (SSR), tạo lợi thế về tốc độ tải bước đầu.
 *   Dữ liệu gốc sau đó truyền xuống component client (AdminPromotionsClient) để render.
 * ===================================================================
 */
import {
 listPromotions as listPromotionsAction,
 type DepositPromotion,
} from "@/actions/promotionActions";
import AdminPromotionsClient from "./promotions-client";

export default async function AdminPromotionsPage() {
 const initialPromotions: DepositPromotion[] = (await listPromotionsAction(false)) ?? [];

 return <AdminPromotionsClient initialPromotions={initialPromotions} />;
}
