'use client';

import { useQRCode } from 'next-qrcode';
import { Copy } from 'lucide-react';
import { GlassCard } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface MfaQrCodeCardProps {
 copyLabel: string;
 manualCodeLabel: string;
 onCopy: (value: string) => void;
 qrCodeUri: string;
 qrColorOptions: Record<string, unknown>;
 secretDisplay: string;
 title: string;
 subtitle: string;
}

export function MfaQrCodeCard({
 copyLabel,
 manualCodeLabel,
 onCopy,
 qrCodeUri,
 qrColorOptions,
 secretDisplay,
 title,
 subtitle,
}: MfaQrCodeCardProps) {
 const { Canvas } = useQRCode();
 return (
  <GlassCard className={cn('!p-8 space-y-8')}>
   <div className={cn('space-y-3 text-center')}>
    <h3 className={cn('text-xl font-black tn-text-primary uppercase italic tracking-tight')}>{title}</h3>
    <p className={cn('tn-text-secondary text-sm font-medium')}>{subtitle}</p>
   </div>
   <div className={cn('flex justify-center tn-surface-strong p-5 rounded-2xl w-fit mx-auto shadow-2xl')}>
    <Canvas text={qrCodeUri} options={{ errorCorrectionLevel: 'M', margin: 2, scale: 4, width: 200, ...qrColorOptions }} />
   </div>
   <div className={cn('text-center space-y-4 pt-4 border-t tn-border')}>
    <p className={cn('tn-text-10 tn-text-tertiary uppercase font-black tracking-widest')}>{manualCodeLabel}</p>
    <div className={cn('flex justify-center items-center gap-3')}>
     <code className={cn('px-5 py-3 tn-panel rounded-xl tn-text-success font-mono tracking-widest font-bold shadow-inner')}>{secretDisplay}</code>
     <button type="button" onClick={() => onCopy(secretDisplay)} aria-label={copyLabel} className={cn('tn-mfa-qr-copy-btn p-3 tn-panel rounded-xl transition-all group')}>
      <Copy className={cn('tn-group-text-primary w-4 h-4 tn-text-secondary transition-colors')} />
     </button>
    </div>
   </div>
  </GlassCard>
 );
}
