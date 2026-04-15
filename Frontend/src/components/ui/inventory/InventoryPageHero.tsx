'use client';

import { PackageSearch } from 'lucide-react';
import { cn } from '@/lib/utils';

interface InventoryPageHeroProps {
 title: string;
 subtitle: string;
}

export default function InventoryPageHero({ title, subtitle }: InventoryPageHeroProps) {
 return (
  <div className={cn('mb-6 flex items-center gap-3')}>
   <div className={cn('rounded-xl border border-slate-200 bg-white/80 p-2 dark:border-slate-700 dark:bg-slate-900/70')}>
    <PackageSearch className={cn('h-5 w-5 text-slate-700 dark:text-slate-100')} />
   </div>
   <div>
    <h1 className={cn('text-2xl font-bold text-slate-900 dark:text-slate-100')}>{title}</h1>
    <p className={cn('text-sm text-slate-600 dark:text-slate-300')}>{subtitle}</p>
   </div>
  </div>
 );
}
