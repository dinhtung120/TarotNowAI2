import { cookies } from 'next/headers';

export async function getServerCookie(name: string): Promise<string | undefined> {
 const cookieStore = await cookies();
 return cookieStore.get(name)?.value;
}

export async function getServerAccessToken(): Promise<string | undefined> {
 return getServerCookie('accessToken');
}

export async function getServerRefreshToken(): Promise<string | undefined> {
 return getServerCookie('refreshToken');
}
