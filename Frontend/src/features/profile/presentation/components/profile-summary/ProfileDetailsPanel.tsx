'use client';

import { cn } from '@/lib/utils';
import { ProfileActionsRow } from './ProfileActionsRow';
import { ProfileBadgesRow } from './ProfileBadgesRow';
import { ProfileIdentity } from './ProfileIdentity';

interface ProfileDetailsPanelProps {
 adminPortalLabel: string;
 displayName: string;
 isAdmin: boolean;
 isTarotReader: boolean;
 levelLabel: string;
 levelValue: string;
 numerologyLabel: string;
 numerologyValue: string;
 onOpenAdminPortal: () => void;
 username: string;
 zodiacLabel: string;
 zodiacValue: string;
}

export function ProfileDetailsPanel(props: ProfileDetailsPanelProps) {
 return (
  <div className={cn('flex-1 text-center md:text-left space-y-5')}>
   <ProfileIdentity displayName={props.displayName} username={props.username} />
   <ProfileBadgesRow levelLabel={props.levelLabel} levelValue={props.levelValue} zodiacLabel={props.zodiacLabel} zodiacValue={props.zodiacValue} numerologyLabel={props.numerologyLabel} numerologyValue={props.numerologyValue} />
   <ProfileActionsRow adminPortalLabel={props.adminPortalLabel} isAdmin={props.isAdmin} isTarotReader={props.isTarotReader} onOpenAdminPortal={props.onOpenAdminPortal} />
  </div>
 );
}
