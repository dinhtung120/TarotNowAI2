import { useMemo } from "react";
import {
  getSessionStorageItem,
  getSessionStorageNumber,
} from "@/shared/storage/browserStorage";

export function useSessionSetup(sessionId: string) {
  return useMemo(
    () => ({
      question: getSessionStorageItem(`question_${sessionId}`, ""),
      cardsToDraw: getSessionStorageNumber(`cardsToDraw_${sessionId}`, 1),
    }),
    [sessionId],
  );
}
