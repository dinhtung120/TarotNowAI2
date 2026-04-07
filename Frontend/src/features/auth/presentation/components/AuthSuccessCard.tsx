'use client';

import type { ReactNode } from 'react';
import { Link } from '@/i18n/routing';
import { Button, GlassCard } from '@/shared/components/ui';
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
  <div className={cn("min-h-dvh flex items-center justify-center bg-[var(--bg-void)] relative overflow-hidden font-sans px-4 py-10")}>
   <div className={`absolute top-[20%] right-[30%] w-96 h-96 rounded-full filter blur-[120px] opacity-40 animate-pulse ${glowClass}`} />
   <GlassCard className={cn("relative z-10 w-full max-w-md p-10 text-center animate-in zoom-in-95 duration-700")}>
    <div className={`w-20 h-20 rounded-2xl flex items-center justify-center mx-auto mb-6 animate-pulse ${iconWrapperClass}`}>
     {icon}
    </div>
    <h2 className={cn("text-3xl font-black italic tracking-tighter tn-text-primary mb-4 uppercase")}>{title}</h2>
    <p className={cn("tn-text-secondary font-medium mb-8 leading-relaxed")}>{description}</p>
    <Link href={ctaHref} tabIndex={-1}>
     <Button variant="brand" size="lg" fullWidth>
      {ctaLabel}
     </Button>
    </Link>
   </GlassCard>
  </div>
 );
}
