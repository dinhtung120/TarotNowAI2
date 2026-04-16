'use client';

/* 
 * Import các thành phần hỗ trợ UI.
 */
import { cn } from '@/lib/utils';

/**
 * Interface cho các tùy chọn card mục tiêu.
 */
export interface CardOption {
  id: number;
  name: string;
}

/**
 * Props của thành phần UseItemCardSelector.
 */
interface UseItemCardSelectorProps {
  label: string;
  value: number | '';
  cardOptions: CardOption[];
  onChange: (value: number | '') => void;
}

/**
 * UseItemCardSelector - Thành phần cho phép người dùng chọn lá bài mục tiêu để sử dụng vật phẩm.
 * Sử dụng các class 'tn-field' để đồng bộ với thiết kế Input của hệ thống.
 */
export default function UseItemCardSelector({
  label,
  value,
  cardOptions,
  onChange,
}: UseItemCardSelectorProps) {
  return (
    <div className="flex flex-col gap-2.5">
      {/* Nhãn mô tả mục tiêu */}
      <span className={cn('tn-text-muted text-[10px] font-black uppercase tracking-widest pl-1')}>
        {label}
      </span>
      
      {/* 
          Ô chọn (Select): 
          Áp dụng 'tn-field' và 'tn-field-accent' để tạo hiệu ứng khi focus cực kỳ bắt mắt.
      */}
      <select
        value={value}
        onChange={(event) => onChange(event.target.value ? Number(event.target.value) : '')}
        className={cn(
          'tn-field tn-field-accent w-full rounded-2xl px-4 py-3 text-sm font-bold transition-all',
          'cursor-pointer hover:bg-white/[0.04]'
        )}
      >
        <option value="" className="bg-[var(--bg-surface)]">--- Chọn lá bài mục tiêu ---</option>
        {cardOptions.map((card) => (
          <option key={card.id} value={card.id} className="bg-[var(--bg-surface)]">
            {card.name}
          </option>
        ))}
      </select>
    </div>
  );
}
