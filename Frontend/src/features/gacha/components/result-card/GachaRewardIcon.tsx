'use client';

import Image from 'next/image';
import { cn } from '@/lib/utils';
import type { SpinGachaItemResult } from '../../gacha.types';
import { GachaFallbackIcon } from './GachaFallbackIcon';

interface GachaRewardIconProps {
 iconLoadFailed: boolean;
 item: SpinGachaItemResult;
 name: string;
 onError: () => void;
 sizeClass: string;
 textClass: string;
}

export function GachaRewardIcon({
 iconLoadFailed,
 item,
 name,
 onError,
 sizeClass,
 textClass,
}: GachaRewardIconProps) {
 if (item.displayIcon && !iconLoadFailed) {
  return (
   <Image
    src={`/images/gacha/${item.displayIcon}`}
    alt={name}
    width={192}
    height={192}
    unoptimized
    className={cn(sizeClass, 'object-contain drop-shadow-2xl')}
    onError={onError}
   />
  );
 }

 return <GachaFallbackIcon item={item} sizeClass={sizeClass} textClass={textClass} />;
}
