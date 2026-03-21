export interface ActionSuccess<T> {
 success: true;
 data?: T;
 error?: undefined;
}

export interface ActionFailure {
 success: false;
 error: string;
 data?: undefined;
}

export type ActionResult<T> = ActionSuccess<T> | ActionFailure;

export function actionOk<T>(data?: T): ActionSuccess<T> {
 return { success: true, data };
}

export function actionFail(error: string): ActionFailure {
 return { success: false, error };
}
