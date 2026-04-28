import { cn } from '@/lib/utils';

export const leaderboardPageClasses = {
 root: cn('relative', 'min-h-screen', 'overflow-hidden', 'bg-slate-950', 'px-4', 'py-6', 'sm:px-6', 'sm:py-8'),
 glowTop: cn('pointer-events-none', 'absolute', 'inset-x-0', 'top-0', 'h-96', 'bg-gradient-to-b', 'from-indigo-900/20', 'to-slate-950'),
 glowLeft: cn('pointer-events-none', 'absolute', '-left-12', '-top-12', 'h-64', 'w-64', 'sm:h-80', 'sm:w-80', 'rounded-full', 'bg-emerald-600/10', 'blur-3xl'),
 glowRight: cn('pointer-events-none', 'absolute', '-right-12', 'top-20', 'h-72', 'w-56', 'sm:h-96', 'sm:w-72', 'rounded-full', 'bg-indigo-600/10', 'blur-3xl'),
 main: cn('relative', 'z-10', 'mx-auto', 'max-w-4xl'),
 header: cn('mb-10', 'flex', 'flex-col', 'items-start', 'gap-4', 'text-left', 'sm:mb-12', 'sm:flex-row', 'sm:items-center', 'sm:gap-6'),
 iconWrap: cn('rounded-2xl', 'bg-gradient-to-br', 'from-emerald-500', 'to-indigo-600', 'p-3', 'sm:p-4', 'shadow-xl', 'shadow-emerald-500/20', 'ring-1', 'ring-white/10'),
 title: cn('bg-gradient-to-r', 'from-white', 'to-slate-400', 'bg-clip-text', 'text-3xl', 'sm:text-5xl', 'font-black', 'tracking-tight', 'text-transparent', 'text-white'),
 desc: cn('mt-2', 'max-w-2xl', 'text-base', 'text-slate-400', 'sm:mt-3', 'sm:text-lg'),
};
