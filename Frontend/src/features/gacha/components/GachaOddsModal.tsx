"use client";

/*
 * ===================================================================
 * COMPONENT: GachaOddsModal
 * ===================================================================
 * MỤC ĐÍCH: Bảng tỷ lệ hiển thị chi tiết phần trăm các món đồ theo luật công khai.
 * Phụ thuộc: Modal, Badge, SkeletonLoader từ hệ thống Shared UI.
 */

import { useTranslations } from 'next-intl';
import { useGachaOdds } from '../hooks/useGacha';
import Modal from '@/shared/components/ui/Modal';
import SkeletonLoader from '@/shared/components/ui/SkeletonLoader';
import Badge from '@/shared/components/ui/Badge';

interface GachaOddsModalProps {
  bannerCode: string;
  isOpen: boolean;
  onClose: () => void;
}

export function GachaOddsModal({ bannerCode, isOpen, onClose }: GachaOddsModalProps) {
  const t = useTranslations('gacha');
  const tCommon = useTranslations('common');
  const { data: oddsData, isLoading, error } = useGachaOdds(bannerCode, isOpen);

  const isVi = t('lang') === 'vi';

  const getRarityVariant = (rarity: string) => {
    switch (rarity.toLowerCase()) {
      case 'legendary': return 'amber';
      case 'epic': return 'purple';
      case 'rare': return 'info';
      default: return 'default';
    }
  };

  return (
    <Modal 
      isOpen={isOpen} 
      onClose={onClose} 
      title={t('oddsTitle')}
      size="md"
    >
      <div className="text-stone-100">
        {isLoading ? (
          <div className="space-y-3 py-4">
            <SkeletonLoader type="text" count={3} />
          </div>
        ) : error ? (
          <div className="text-red-400 py-4 text-sm text-center">{tCommon('error')}</div>
        ) : oddsData ? (
          <div className="space-y-4">
            <div className="text-[10px] uppercase tracking-widest text-stone-500 mb-2">
              {t('oddsVersion')}: {oddsData.oddsVersion}
            </div>
            
            <div className="h-[350px] overflow-y-auto custom-scrollbar rounded-2xl border border-stone-800 p-2 bg-stone-900/50">
              <table className="w-full text-sm border-separate border-spacing-y-1">
                <thead>
                  <tr className="text-[10px] uppercase tracking-widest text-stone-500">
                    <th className="text-left font-black py-2 px-3">{t('item')}</th>
                    <th className="text-center font-black py-2 px-3">{t('rarity')}</th>
                    <th className="text-right font-black py-2 px-3">{t('probability')}</th>
                  </tr>
                </thead>
                <tbody>
                  {oddsData.items.map((item, idx) => (
                    <tr key={idx} className="group hover:bg-stone-800/40 transition-colors">
                      <td className="py-3 px-3 font-semibold text-stone-300">
                        {isVi ? item.displayNameVi : item.displayNameEn}
                      </td>
                      <td className="py-3 px-3 text-center">
                        <Badge variant={getRarityVariant(item.rarity)}>
                          {item.rarity}
                        </Badge>
                      </td>
                      <td className="py-3 px-3 text-right text-stone-400 font-mono">
                        {item.probabilityPercent.toFixed(2)}%
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        ) : null}
      </div>
    </Modal>
  );
}
