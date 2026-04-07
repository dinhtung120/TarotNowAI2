import { cn } from '@/lib/utils';

interface StreakFreezeSummaryProps {
  freezePrice: number;
  minutes: number;
  hours: number;
  preBreakStreak: number;
}

export default function StreakFreezeSummary({ freezePrice, minutes, hours, preBreakStreak }: StreakFreezeSummaryProps) {
  return (
    <>
      <div className={cn('text-center')}>
        <div className={cn('mx-auto mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-blue-500/20 text-blue-400')}>
          <svg className={cn('h-8 w-8')} fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
        </div>
        <h3 className={cn('mb-2 text-xl font-bold text-white')}>Đóng băng Streak</h3>
        <p className={cn('mb-4 text-sm text-slate-400')}>Bạn đã đánh mất chuỗi {preBreakStreak} ngày kiên trì! Khôi phục ngay trước khi quá muộn.</p>
      </div>
      <div className={cn('mb-6 rounded-xl bg-slate-800/50 p-3')}>
        <div className={cn('flex items-center justify-between text-sm')}>
          <span className={cn('text-slate-300')}>Giá chuộc (Kim Cương)</span>
          <span className={cn('flex items-center gap-1 font-bold text-cyan-400')}>
            <svg className={cn('h-4 w-4')} viewBox="0 0 24 24" fill="currentColor"><path d="M11 2L2 14.5L12 22L22 14.5L13 2H11ZM12 4.4L18.4 13L12 18.5L5.6 13L12 4.4Z" /></svg>
            {freezePrice}
          </span>
        </div>
        <div className={cn('mt-2 flex items-center justify-between text-sm')}>
          <span className={cn('text-slate-300')}>Thời gian còn lại</span>
          <span className={cn('font-semibold text-red-400')}>{hours}h {minutes}m</span>
        </div>
      </div>
    </>
  );
}
