'use client';

import { Award, Coins, Diamond, Sparkles } from 'lucide-react';
import { cn } from '@/lib/utils';
import type { SpinGachaItemResult } from '../../gacha.types';

interface GachaFallbackIconProps {
 item: SpinGachaItemResult;
 sizeClass: string;
 textClass: string;
}

export function GachaFallbackIcon({
 item,
 sizeClass,
 textClass,
}: GachaFallbackIconProps) {
 const iconProps = { className: cn(sizeClass, textClass) };
 if (item.rewardType === 'diamond') return <Diamond {...iconProps} />;
 if (item.rewardType === 'gold') return <Coins {...iconProps} />;
 if (item.rewardType === 'title') return <Award {...iconProps} />;
 return <Sparkles {...iconProps} />;
}
