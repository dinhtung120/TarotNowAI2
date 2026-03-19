/*
 * ===================================================================
 * COMPONENT/FILE: Tarot Metadata (tarotData.ts)
 * BỐI CẢNH (CONTEXT):
 *   Nơi lưu trữ siêu dữ liệu (Metadata) cốt lõi của bộ bài Tarot 78 lá.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Phân loại (Mapping) ID của lá bài thành các Chất (Suit) khác nhau: 
 *     Major (Ẩn chính) và Minor (Wands, Cups, Swords, Pentacles).
 *   - Khởi tạo danh sách chuẩn `TAROT_DECK` gồm 78 lá.
 *   - Các thông tin về Tên lá bài và Ý nghĩa chi tiết được chuyển giao cho hệ thống 
 *     Đa ngôn ngữ (next-intl) xử lý thay vì hardcode ở đây.
 * ===================================================================
 */
export type TarotSuitKey = 'major' | 'wands' | 'cups' | 'swords' | 'pentacles';

export interface TarotCardMeta {
  id: number;
  suit: TarotSuitKey;
}

export const TAROT_CARD_COUNT = 78;

export function tarotSuitKeyFromId(id: number): TarotSuitKey {
  if (id <= 21) return 'major';
  if (id <= 35) return 'wands';
  if (id <= 49) return 'cups';
  if (id <= 63) return 'swords';
  return 'pentacles';
}

export const TAROT_DECK: TarotCardMeta[] = Array.from(
  { length: TAROT_CARD_COUNT },
  (_, id) => ({ id, suit: tarotSuitKeyFromId(id) }),
);

