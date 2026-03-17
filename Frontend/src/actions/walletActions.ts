'use server';

import { cookies } from 'next/headers';
import { PaginatedList, WalletBalance, WalletTransaction } from '@/types/wallet';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5037/api/v1';

export async function getWalletBalance(): Promise<WalletBalance | null> {
 const cookieStore = await cookies();
 // Normally auth logic attaches tokens. In a standard setup with HTTP-Only cookies, we might just pass credentials if same-site.
 // However, if we're using a Bearer token stored in cookies, we append it here.
 const accessToken = cookieStore.get('accessToken')?.value; // Replace with actual token retrieval method

 try {
 const response = await fetch(`${API_URL}/Wallet/balance`, {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store', // Always fetch fresh balance
 });

 if (!response.ok) {
 console.error('getWalletBalance error', response.status);
 return null;
 }

 return await response.json();
 } catch (error) {
 console.error('Failed to get wallet balance:', error);
 return null;
 }
}

export async function getLedger(page = 1, limit = 20): Promise<PaginatedList<WalletTransaction> | null> {
 const cookieStore = await cookies();
 const accessToken = cookieStore.get('accessToken')?.value;

 try {
 const response = await fetch(`${API_URL}/Wallet/ledger?page=${page}&limit=${limit}`, {
 method: 'GET',
 headers: {
 'Authorization': `Bearer ${accessToken}`,
 'Content-Type': 'application/json',
 },
 cache: 'no-store',
 });

 if (!response.ok) {
 console.error('getLedger error', response.status);
 return null;
 }

 return await response.json();
 } catch (error) {
 console.error('Failed to get ledger:', error);
 return null;
 }
}
