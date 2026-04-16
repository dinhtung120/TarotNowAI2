'use client';

/* 
 * Import các thành phần hỗ trợ UI.
 * - PackageSearch: Icon trực quan cho kho đồ từ thư viện lucide-react.
 */
import { PackageSearch } from 'lucide-react';
import { cn } from '@/lib/utils';

/**
 * Props của thành phần InventoryPageHero.
 */
interface InventoryPageHeroProps {
  title: string;
  subtitle: string;
}

/**
 * InventoryPageHero - Thành phần hiển thị phần giới thiệu (Hero) của trang Kho đồ.
 * Sử dụng phong cách thiết kế cao cấp với tiêu đề Metallic.
 */
export default function InventoryPageHero({ title, subtitle }: InventoryPageHeroProps) {
  return (
    <div className={cn('relative mb-12 flex flex-col items-center gap-6 text-center sm:flex-row sm:text-left')}>
      {/* 
          Icon Container với hiệu ứng Glassmorphism và Glow 
      */}
      <div className={cn(
        'group relative flex h-20 w-20 shrink-0 items-center justify-center rounded-3xl border tn-border-soft',
        'bg-white/[0.03] shadow-[0_0_20px_rgba(139,92,246,0.1)] transition-all duration-700 hover:scale-110 hover:tn-border-accent'
      )}>
        <div className={cn('absolute inset-0 rounded-3xl bg-gradient-to-br from-violet-500/20 to-fuchsia-500/20 opacity-0 transition-opacity duration-700 group-hover:opacity-100')} />
        <PackageSearch className={cn('relative h-10 w-10 tn-text-primary transition-colors duration-500 group-hover:tn-text-accent')} />
        
        {/* Decorative elements */}
        <div className={cn('absolute -right-1 -top-1 h-3 w-3 rounded-full bg-violet-400 blur-[2px] animate-pulse')} />
        <div className={cn('absolute -bottom-1 -left-1 h-2 w-2 rounded-full bg-fuchsia-400 blur-[1px] animate-pulse delay-75')} />
      </div>
      
      <div className="flex flex-col gap-2">
        {/* 
            Tiêu đề Lunar Metallic 
        */}
        <h1 className={cn(
          'lunar-metallic-text text-4xl font-black uppercase tracking-[0.15em] sm:text-5xl lg:text-6xl',
          'drop-shadow-[0_2px_10px_rgba(0,0,0,0.5)]'
        )}>
          {title}
        </h1>
        
        <div className="flex items-center gap-3">
          <div className="h-[1px] w-8 bg-gradient-to-r from-transparent via-violet-500 to-transparent sm:w-12" />
          <p className={cn('tn-text-secondary text-sm font-bold tracking-widest uppercase opacity-60')}>
            {subtitle}
          </p>
          <div className="h-[1px] w-8 bg-gradient-to-r from-transparent via-fuchsia-500 to-transparent sm:w-12" />
        </div>
      </div>

      {/* Background glow behind hero */}
      <div className={cn('absolute -left-20 -top-20 -z-10 h-64 w-64 rounded-full bg-violet-600/10 blur-[100px]')} />
    </div>
  );
}
