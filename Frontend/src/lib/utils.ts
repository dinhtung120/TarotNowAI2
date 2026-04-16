import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

export function cn(...inputs: ClassValue[]): string {
  return twMerge(clsx(inputs));
}

/**
 * Định dạng số để hiển thị trên UI lá bài.
 * - Loại bỏ các phần thập phân là .00 (ví dụ: 10.00 -> 10).
 * - Giảm bớt dung lượng số nếu quá lớn (ví dụ: rút gọn chữ số thập phân).
 */
export function formatCardStat(value: number | string): string {
  const num = typeof value === 'string' ? parseFloat(value) : value;
  if (isNaN(num)) return value.toString();

  // Chuyển về chuỗi và loại bỏ .00 dư thừa
  return parseFloat(num.toFixed(2)).toString();
}

