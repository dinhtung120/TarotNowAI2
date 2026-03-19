/*
 * ===================================================================
 * COMPONENT/FILE: Auth Client Utilities (auth-client.ts)
 * BỐI CẢNH (CONTEXT):
 *   Các hàm tiện ích hoạt động TẠI CLIENT-SIDE (Trình duyệt) để thao tác với Auth.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Cung cấp hàm `getAccessToken` để bóc tách và lấy mã Access Token từ chuỗi Cookie 
 *     của trình duyệt.
 *   - Vì Action đăng nhập (authActions) đã set Cookie accessToken với `httpOnly: false`, 
 *     nên Client có toàn quyền đọc được giá trị này để đính kèm vào Header Authorization 
 *     khi gọi API trực tiếp.
 * ===================================================================
 */
export const getAccessToken = (): string | null => {
 if (typeof document === 'undefined') return null;
 const name = 'accessToken=';
 const decodedCookie = decodeURIComponent(document.cookie);
 const ca = decodedCookie.split(';');
 for (let i = 0; i < ca.length; i++) {
 let c = ca[i];
 while (c.charAt(0) === ' ') {
 c = c.substring(1);
 }
 if (c.indexOf(name) === 0) {
 return c.substring(name.length, c.length);
 }
 }
 return null;
};
