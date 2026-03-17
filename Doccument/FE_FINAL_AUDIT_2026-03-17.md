# FE Final Audit Report - TarotNowAI2

- Date: 2026-03-17
- Scope: `Frontend/src`
- Verification commands:
  - `npm run lint` -> PASS
  - `npm run build` -> PASS

## Findings (ordered by severity)

### P1 - Logic/Security/Runtime risk

1. **Redirect loop risk cho route `/admin` không có locale**
- File: `Frontend/src/proxy.ts:38`, `Frontend/src/proxy.ts:40`, `Frontend/src/proxy.ts:51`, `Frontend/src/proxy.ts:57`
- Vấn đề: lấy locale bằng `pathname.split('/')[1]` trên path `/admin` cho ra `admin`, sau đó redirect sang `/admin/login`.
- Tác động: `/admin/login` vẫn match regex admin, không có token sẽ tiếp tục redirect về chính nó -> loop.
- Gợi ý: chỉ nhận locale hợp lệ từ `routing.locales`, fallback `routing.defaultLocale`; hoặc ép mọi admin path về `/{defaultLocale}/admin`.

2. **Server Action ghi debug file vào `/tmp` + log metadata token trong production path**
- File: `Frontend/src/actions/adminActions.ts:4`, `Frontend/src/actions/adminActions.ts:287-296`, `Frontend/src/actions/adminActions.ts:312-332`
- Vấn đề: `writeFileSync('/tmp/deposit_debug.log', ...)` trong `processDeposit`, kèm log trạng thái token/response body.
- Tác động: tăng I/O không cần thiết, khó tương thích môi trường serverless/read-only, có rủi ro lộ thông tin vận hành.
- Gợi ý: bỏ hoàn toàn `debugLog` local file; dùng logger chuẩn theo environment và redaction.

3. **Khởi tạo SSE phụ thuộc token nhưng effect không theo dõi token**
- File: `Frontend/src/components/AiInterpretationStream.tsx:101-103`, `Frontend/src/components/AiInterpretationStream.tsx:156-167`
- Vấn đề: URL stream ghép `access_token=${accessToken}`, nhưng effect init chỉ phụ thuộc `sessionId` (kèm `eslint-disable`).
- Tác động: nếu token hydrate chậm/đổi mới sau mount, stream có thể mở với token rỗng/sai và không tự khởi động lại.
- Gợi ý: gate theo `accessToken` hợp lệ trước khi open stream, hoặc thêm cơ chế retry khi token thay đổi.

### P2 - Theme/Architecture/Consistency

4. **CSS runtime invalid: ghép opacity suffix trực tiếp vào `var(--token)`**
- File:
  - `Frontend/src/app/[locale]/page.tsx:32`
  - `Frontend/src/app/[locale]/chat/page.tsx:161`
  - `Frontend/src/app/[locale]/wallet/withdraw/page.tsx:292`
- Vấn đề: pattern kiểu `var(--success)30` hoặc `var(--... )20` không phải CSS color hợp lệ.
- Tác động: border color có thể bị browser bỏ qua, UI không đúng thiết kế.
- Gợi ý: dùng token alpha riêng (`--...-bg`) hoặc `color-mix(...)`.

5. **i18n coverage chưa đồng bộ trên route có locale**
- Mô tả: nhiều trang trong `app/[locale]` vẫn hardcode text (VI/EN) mà không dùng `useTranslations/getTranslations`.
- Ví dụ:
  - `Frontend/src/app/[locale]/chat/page.tsx:106-117`
  - `Frontend/src/app/[locale]/admin/deposits/page.tsx:187-199`
  - `Frontend/src/app/[locale]/(auth)/login/page.tsx:75-76`, `:86`, `:95`, `:104`, `:135`, `:140-143`
- Tác động: đổi locale không đổi nội dung đồng bộ, tăng hardcode và khó bảo trì nội dung.
- Gợi ý: chuẩn hóa message keys theo namespace cho toàn bộ page trong `app/[locale]`.

6. **Redirect sau login hardcode `'/'`, có thể làm lệch locale hiện tại**
- File: `Frontend/src/app/[locale]/(auth)/login/page.tsx:67`
- Vấn đề: dùng `window.location.assign('/')`.
- Tác động: người dùng đang ở locale khác có thể bị trả về locale mặc định qua middleware.
- Gợi ý: dùng router locale-aware (`useRouter`) hoặc redirect về `/${currentLocale}`.

7. **`JSON.parse` không guard dữ liệu backend**
- File: `Frontend/src/app/[locale]/reading/history/[id]/page.tsx:83`
- Vấn đề: parse trực tiếp `detail.cardsDrawn` không try/catch.
- Tác động: payload lỗi định dạng có thể làm crash render trang lịch sử chi tiết.
- Gợi ý: bọc parse an toàn + fallback mảng rỗng.

8. **Cấu hình API base bị lặp nhiều nơi + fallback port không nhất quán**
- Files (đại diện):
  - `Frontend/src/actions/*` (nhiều file định nghĩa lại `API_URL`)
  - `Frontend/src/components/AiInterpretationStream.tsx:101` (fallback `5221`)
  - các action khác fallback `5037`
- Tác động: khó thay đổi môi trường, dễ lệch endpoint giữa module.
- Gợi ý: gom 1 nguồn `apiBaseUrl` trong `lib` và dùng thống nhất.

### P3 - Cleanup/Code hygiene

9. **Còn `eslint-disable` cục bộ và TODO chưa đóng**
- File:
  - `Frontend/src/components/AiInterpretationStream.tsx:166`
  - `Frontend/src/components/common/Navbar.tsx:184`
- Tác động: tăng nợ kỹ thuật; logic init stream phụ thuộc comment bypass lint.
- Gợi ý: refactor effect để bỏ disable; TODO cần ticket/link rõ ràng.

## Additional Notes

- Không còn hardcoded màu dạng `#hex`/`rgba(...)` rải rác trong TSX theo scan hiện tại (ngoài token ở `globals.css`).
- Không còn `colorScheme: 'dark'` và không còn `dangerouslySetInnerHTML` trong FE.

## Open Questions / Assumptions

1. Có yêu cầu chính thức hỗ trợ truy cập `/admin` (không locale) hay luôn bắt buộc `/{locale}/admin`?
2. Với SSE reading, token luôn sẵn ngay khi mount hay có thể hydrate trễ (zustand/persist)?
3. Kế hoạch i18n đầy đủ cho admin/chat/auth đã được chốt phạm vi hay chưa?

## Recommended Next Order

1. Sửa toàn bộ P1 trước (proxy loop, bỏ debug file logging, ổn định init SSE theo token).
2. Sửa P2.4 (invalid CSS var opacity) vì ảnh hưởng hiển thị nhiều màn.
3. Chuẩn hóa i18n + API base centralization (P2.5, P2.8).
4. Dọn P3 còn lại (`eslint-disable`, TODO có ticket).

---

## Update - P1 Fix Started (2026-03-17)

- Đã sửa xong 3 mục P1 trong report:
  1. `proxy.ts`: fix locale resolve cho admin-route để tránh redirect loop khi truy cập `/admin` không locale.
  2. `adminActions.ts`: bỏ debug logger ghi file `/tmp/deposit_debug.log` và metadata token trong `processDeposit`.
  3. `AiInterpretationStream.tsx`: chỉ init SSE khi có `accessToken`, theo dõi token trong effect để tránh mở stream với token rỗng.

- Re-verify sau khi sửa:
  - `npm run lint` -> PASS
  - `npm run build` -> PASS

---

## Update - P2/P3 Fix Continued (2026-03-17)

- Đã sửa xong các mục sau trong report:
  - P2.4 `var(--token)20/30/40` (CSS invalid) bằng `color-mix(...)` và token alpha.
  - P2.6 redirect sau login đã locale-aware: `window.location.assign(\`/${locale}\`)`.
  - P2.7 `JSON.parse(detail.cardsDrawn)` đã được guard bằng `useMemo` + `try/catch`.
  - P2.8 gom API base URL về `Frontend/src/lib/api.ts`, thay thế toàn bộ `process.env.NEXT_PUBLIC_API_URL || ...` rải rác.
  - P3.9 loại bỏ `TODO`/`eslint-disable` còn sót; dọn warning `react-hooks/exhaustive-deps` để `lint` sạch 100%.

- Re-verify sau khi sửa:
  - `npm run lint` -> PASS
  - `npm run build` -> PASS
