import { redirect } from 'next/navigation';
import { getLocale } from 'next-intl/server';

/**
 * Trang chủ Dashboard của Admin.
 * 
 * Tại sao cần trang này?
 * - Tránh lỗi 404 khi người dùng truy cập trực tiếp vào /admin.
 * - Cung cấp cái nhìn tổng quan hoặc điều hướng nhanh đến các phần quản lý.
 * 
 * Logic:
 * - Hiện tại ta đơn giản là redirect vào trang quản lý người dùng (/admin/users).
 * - Sau này có thể thêm biểu đồ thống kê doanh thu, người dùng mới tại đây.
 */
export default async function AdminDashboard() {
  const locale = await getLocale();
  
  // Tạm thời redirect sang trang quản lý người dùng là trang quan trọng nhất của Admin
  redirect(`/${locale}/admin/users`);
}
