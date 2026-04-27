export interface ActionSuccess<T> {
 success: true;
 data?: T;
 error?: undefined;
 status?: undefined;
 errorCode?: undefined;
}

export interface ActionFailure {
 success: false;
 error: string;
 status?: number;
 errorCode?: string;
 data?: undefined;
}

export type ActionResult<T> = ActionSuccess<T> | ActionFailure;

export function actionOk<T>(data?: T): ActionSuccess<T> {
 return { success: true, data };
}

interface ActionFailureMeta {
 status?: number;
 errorCode?: string;
}

function normalizeStatus(status: number | undefined): number | undefined {
 if (typeof status !== 'number' || !Number.isFinite(status)) {
  return undefined;
 }

 const normalized = Math.floor(status);
 if (normalized < 100 || normalized > 599) {
  return undefined;
 }

 return normalized;
}

export function actionFail(error: string, meta: ActionFailureMeta = {}): ActionFailure {
 const status = normalizeStatus(meta.status);
 const errorCode = meta.errorCode?.trim();
 return {
  success: false,
  error,
  ...(status ? { status } : {}),
  ...(errorCode ? { errorCode } : {}),
 };
}

export function resolveActionFailureStatus<T>(
 result: ActionResult<T>,
 fallbackStatus = 500,
): number {
 if (result.success) {
  return fallbackStatus;
 }

 const normalized = normalizeStatus(result.status);
 if (!normalized) {
  return fallbackStatus;
 }

 return normalized >= 500 ? normalized : Math.max(400, normalized);
}
