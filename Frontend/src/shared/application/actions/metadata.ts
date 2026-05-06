'use server';

import { getServerAccessToken } from "@/shared/application/gateways/serverAuth";
import { serverHttpRequest } from "@/shared/application/gateways/serverHttpClient";
import { actionFail, actionOk, type ActionResult } from "@/shared/domain/actionResult";
import type { WalletBalance } from "@/features/wallet/shared/types";
import type { IStreakStatusResult } from "@/features/checkin/streak/checkin.types";
import type { ListConversationsResult } from "@/features/chat/shared/actions";
import type { NotificationListResponse } from "@/features/notifications/shared/actions/types";
import { AUTH_ERROR } from "@/shared/domain/authErrors";


export interface UserMetadataDto {
  wallet: WalletBalance;
  streak: IStreakStatusResult;
  unreadNotificationCount: number;
  recentNotifications: NotificationListResponse; 
  unreadChatCount: number;
  activeConversations: ListConversationsResult;
}


export async function getInitialMetadata(): Promise<ActionResult<UserMetadataDto>> {
  const token = await getServerAccessToken();
  if (!token) return actionFail(AUTH_ERROR.UNAUTHORIZED);

  try {
    const result = await serverHttpRequest<UserMetadataDto>("/user-context/metadata", {
      method: "GET",
      token,
      fallbackErrorMessage: "Failed to get aggregate metadata",
    });

    if (!result.ok) {
      return actionFail(result.error || "Failed to get aggregate metadata");
    }

    return actionOk(result.data);
  } catch {
    return actionFail("Metadata fetch error");
  }
}
