/*
 * ===================================================================
 * COMPONENT/FILE: Reader Types (reader.ts)
 * BỐI CẢNH (CONTEXT):
 *   Định nghĩa dữ liệu hồ sơ dành riêng cho Reader (người đọc Tarot).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - `ReaderProfile`: Lưu trữ giá cả, Bio, đánh giá và Trạng thái (Online/Busy) của Reader.
 *   - `ReaderRequest`: Đơn đăng ký trở thành Reader đang chờ Admin xét duyệt.
 *   - DTO cho việc Cập nhật giá/Thông tin cá nhân (UpdateReaderProfileData).
 * ===================================================================
 */
export interface ReaderProfile {
 id: string;
 userId: string;
 displayName: string;
 bio: string;
 introText?: string;
 pricePerQuestion: number;
 rating: number;
 totalReadings: number;
 status: 'Offline' | 'Online' | 'AcceptingQuestions' | 'Busy';
 registeredAt: string;
}

export interface ReaderRequest {
 id: string;
 userId: string;
 introText: string;
 status: 'Pending' | 'Approved' | 'Rejected';
 adminNotes?: string;
 submittedAt: string;
 processedAt?: string;
}

export interface SubmitReaderRequestData {
 introText: string;
}

export interface UpdateReaderProfileData {
 bio?: string;
 introText?: string;
 pricePerQuestion?: number;
}
