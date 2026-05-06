'use client';

import type { ReactNode } from 'react';
import { OptimizedLink as Link } from '@/shared/navigation/useOptimizedLink';
import { Button, GlassCard } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface AuthSuccessCardProps {
 icon: ReactNode;
 title: string;
 description: string;
 ctaLabel: string;
 ctaHref: string;
 glowClass: string;
 iconWrapperClass: string;
}

export function AuthSuccessCard({
 icon,
 title,
 description,
 ctaLabel,
 ctaHref,
 glowClass,
 iconWrapperClass,
}: AuthSuccessCardProps) {
 return (
  <div className={cn("relative", "flex", "min-h-dvh", "items-center", "justify-center", "overflow-hidden", "bg-slate-950", "px-4", "py-10", "font-sans")}>
   <div className={cn("absolute", "right-1/3", "top-1/4", "h-96", "w-96", "rounded-full", "opacity-40", "blur-3xl", "animate-pulse", glowClass)} />
   <GlassCard className={cn("relative", "z-10", "w-full", "max-w-md", "animate-in", "zoom-in-95", "p-10", "text-center", "duration-700")}>
    <div className={cn("mx-auto", "mb-6", "flex", "h-20", "w-20", "items-center", "justify-center", "rounded-2xl", "animate-pulse", iconWrapperClass)}>
     {icon}
    </div>
    <h2 className={cn("mb-4", "text-3xl", "font-black", "uppercase", "italic", "tracking-tighter", "tn-text-primary")}>{title}</h2>
    <p className={cn("mb-8", "leading-relaxed", "font-medium", "tn-text-secondary")}>{description}</p>
    <Link href={ctaHref} tabIndex={-1}>
     <Button variant="brand" size="lg" fullWidth>
      {ctaLabel}
     </Button>
    </Link>
   </GlassCard>
  </div>
 );
}
