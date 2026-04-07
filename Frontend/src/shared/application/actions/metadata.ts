'use server';

import { getServerAccessToken } from "@/shared/infrastructure/auth/serverAuth";
import { serverHttpRequest } from "@/shared/infrastructure/http/serverHttpClient";
import { actionFail, actionOk, type ActionResult } from "@/shared/domain/actionResult";
import type { WalletBalance } from "@/features/wallet/domain/types";
import type { IStreakStatusResult } from "@/features/checkin/types/checkin.types";
import type { ListConversationsResult } from "@/features/chat/application/actions";
import type { NotificationListResponse } from "@/features/notifications/application/actions/types";


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
  if (!token) return actionFail("Unauthorized");

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
