'use client';

import type { useProfilePage } from '@/features/profile/application/useProfilePage';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import { ProfileAvatarUploader } from './profile-summary/ProfileAvatarUploader';
import { ProfileDetailsPanel } from './profile-summary/ProfileDetailsPanel';

type ProfilePageState = ReturnType<typeof useProfilePage>;

interface ProfileSummaryCardProps {
 t: ProfilePageState['t'];
 tCommon: ProfilePageState['tCommon'];
 router: ProfilePageState['router'];
 profileData: ProfilePageState['profileData'];
 avatarPreview: ProfilePageState['avatarPreview'];
 avatarUploading: ProfilePageState['avatarUploading'];
 isSubmitting: ProfilePageState['isSubmitting'];
 isAdmin: boolean;
 isTarotReader: boolean;
 handleAvatarSelect: ProfilePageState['handleAvatarSelect'];
}

export function ProfileSummaryCard({
 t,
 tCommon,
 router,
 profileData,
 avatarPreview,
 avatarUploading,
 isSubmitting,
 isAdmin,
 isTarotReader,
 handleAvatarSelect,
}: ProfileSummaryCardProps) {
 if (!profileData) {
  return null;
 }

 return (
  <GlassCard className={cn('!p-6 overflow-hidden relative group')}>
   <div className={cn('relative z-10 tn-profile-summary-row gap-8')}>
    <ProfileAvatarUploader
     avatarAlt={tCommon('avatar_alt')}
     avatarPreview={avatarPreview}
     avatarUploading={avatarUploading}
     displayName={profileData.displayName}
     isSubmitting={isSubmitting}
     onAvatarSelect={handleAvatarSelect}
     uploadLabel={t('avatar_upload') || 'Đổi ảnh'}
    />
    <ProfileDetailsPanel
     adminPortalLabel={t('admin_portal')}
     displayName={profileData.displayName}
     isAdmin={isAdmin}
     isTarotReader={isTarotReader}
     levelLabel={t('level')}
     levelValue={String(profileData.level)}
     numerologyLabel={t('numerology')}
     numerologyValue={String(profileData.numerology)}
     onOpenAdminPortal={() => router.push('/admin/users')}
     username={profileData.username}
     zodiacLabel={t('zodiac')}
     zodiacValue={profileData.zodiac}
    />
   </div>
  </GlassCard>
 );
}
