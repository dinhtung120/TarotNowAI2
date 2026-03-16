/**
 * Tiện ích hỗ trợ lấy Token từ Cookie ở phía Client.
 * Vì accessToken được set httpOnly: false trong authActions.ts,
 * nên client-side component có thể đọc được để gửi trong Header Authorization.
 */
export const getAccessToken = (): string | null => {
    if (typeof document === 'undefined') return null;
    
    const name = 'accessToken=';
    const decodedCookie = decodeURIComponent(document.cookie);
    const ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return null;
};
