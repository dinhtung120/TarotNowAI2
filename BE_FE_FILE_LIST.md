# Danh sach file BE va FE kem mo ta tac dung

Danh sach duoi day duoc lay tu cac file dang duoc git theo doi trong du an.
Moi dong gom duong dan file va comment mo ta tac dung chinh.

## Backend
- `Backend/BE_AUDIT_FINDINGS_2026-03-18.md` - Bao cao audit backend theo dot, tong hop van de va muc do anh huong.
- `Backend/BE_FINAL_AUDIT_REPORT.md` - Bao cao audit tong hop cuoi cung cho backend.
- `Backend/TarotNow.slnx` - Solution file gom cac project .NET cua backend de build/test dong bo.
- `Backend/cookies.txt` - File luu cookie phuc vu test API thu cong hoac script local.

- `Backend/src/TarotNow.Api/Properties/launchSettings.json` - Cau hinh profile chay/debug local cho project API.
- `Backend/src/TarotNow.Api/TarotNow.Api.csproj` - Cau hinh project API: package reference, target framework, build settings.
- `Backend/src/TarotNow.Api/TarotNow.Api.http` - Bo request mau de goi thu endpoint API tu IDE/rest client.
- `Backend/src/TarotNow.Api/appsettings.Development.json` - Cau hinh moi truong Development (db, jwt, redis, provider...) cho API.
- `Backend/src/TarotNow.Api/appsettings.json` - Cau hinh mac dinh cua API, duoc override theo env.









- `Backend/src/TarotNow.Application/Features/Escrow/Commands/AcceptOffer/AcceptOfferCommand.cs` - Command model mo ta request ghi/thao tac cho use-case 'AcceptOffer' thuoc module escrow/giu tien giao dich.


- `Backend/src/TarotNow.Application/Features/Escrow/Queries/GetEscrowStatus/GetEscrowStatusQuery.cs` - Query model mo ta yeu cau doc du lieu cho use-case 'GetEscrowStatus' thuoc module escrow/giu tien giao dich.



















- `Backend/src/TarotNow.Application/Features/Wallet/Queries/WalletDtos.cs` - Thanh phan use-case 'WalletDtos.cs' trong module vi va so cai (CQRS pipeline).
- `Backend/src/TarotNow.Application/Features/Withdrawal/Commands/CreateWithdrawal/CreateWithdrawalCommand.cs` - Command model mo ta request ghi/thao tac cho use-case 'CreateWithdrawal' thuoc module rut tien.
- `Backend/src/TarotNow.Application/Features/Withdrawal/Commands/ProcessWithdrawal/ProcessWithdrawalCommand.cs` - Command model mo ta request ghi/thao tac cho use-case 'ProcessWithdrawal' thuoc module rut tien.
- `Backend/src/TarotNow.Application/Features/Withdrawal/Queries/ListWithdrawals/ListWithdrawalsQuery.cs` - Query model mo ta yeu cau doc du lieu cho use-case 'ListWithdrawals' thuoc module rut tien.
- `Backend/src/TarotNow.Application/Interfaces/IAdminRepository.cs` - Interface abstraction 'AdminRepository' de tach Application khoi implementation ha tang (DI/test de mock).
- `Backend/src/TarotNow.Application/Interfaces/IAiProvider.cs` - Interface abstraction 'AiProvider' de tach Application khoi implementation ha tang (DI/test de mock).


































- `Backend/tests/TarotNow.Application.UnitTests/Admin/GetLedgerMismatchQueryHandlerTests.cs` - Unit test 'GetLedgerMismatchQueryHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Admin/ApproveReaderCommandHandlerTests.cs` - Unit test 'ApproveReaderCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Admin/ProcessDepositCommandHandlerTests.cs` - Unit test 'ProcessDepositCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Auth/Commands/ForgotPasswordCommandHandlerTests.cs` - Unit test 'ForgotPasswordCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Auth/Commands/LoginCommandHandlerTests.cs` - Unit test 'LoginCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Auth/Commands/RegisterCommandHandlerTests.cs` - Unit test 'RegisterCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Auth/Commands/ResetPasswordCommandHandlerTests.cs` - Unit test 'ResetPasswordCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Auth/Commands/SendEmailVerificationOtpCommandHandlerTests.cs` - Unit test 'SendEmailVerificationOtpCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Auth/Commands/VerifyEmailCommandHandlerTests.cs` - Unit test 'VerifyEmailCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Chat/CreateConversationCommandHandlerTests.cs` - Unit test 'CreateConversationCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Chat/CreateReportCommandHandlerTests.cs` - Unit test 'CreateReportCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Chat/MarkMessagesReadCommandHandlerTests.cs` - Unit test 'MarkMessagesReadCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Chat/SendMessageCommandHandlerTests.cs` - Unit test 'SendMessageCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Escrow/AcceptOfferCommandHandlerTests.cs` - Unit test 'AcceptOfferCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Escrow/AddQuestionCommandHandlerTests.cs` - Unit test 'AddQuestionCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Escrow/ConfirmReleaseCommandHandlerTests.cs` - Unit test 'ConfirmReleaseCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Escrow/OpenDisputeCommandHandlerTests.cs` - Unit test 'OpenDisputeCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Escrow/ReaderReplyCommandHandlerTests.cs` - Unit test 'ReaderReplyCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/History/Queries/GetReadingDetailQueryHandlerTests.cs` - Unit test 'GetReadingDetailQueryHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/History/Queries/GetReadingHistoryQueryHandlerTests.cs` - Unit test 'GetReadingHistoryQueryHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Mfa/MfaChallengeCommandHandlerTests.cs` - Unit test 'MfaChallengeCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Mfa/MfaSetupCommandHandlerTests.cs` - Unit test 'MfaSetupCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Mfa/MfaVerifyCommandHandlerTests.cs` - Unit test 'MfaVerifyCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Reader/SubmitReaderRequestCommandHandlerTests.cs` - Unit test 'SubmitReaderRequestCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Reader/UpdateReaderProfileCommandHandlerTests.cs` - Unit test 'UpdateReaderProfileCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Reader/UpdateReaderStatusCommandHandlerTests.cs` - Unit test 'UpdateReaderStatusCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Withdrawal/CreateWithdrawalCommandHandlerTests.cs` - Unit test 'CreateWithdrawalCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Features/Withdrawal/ProcessWithdrawalCommandHandlerTests.cs` - Unit test 'ProcessWithdrawalCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Reading/InitReadingSessionCommandHandlerTests.cs` - Unit test 'InitReadingSessionCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Reading/RevealReadingSessionCommandHandlerTests.cs` - Unit test 'RevealReadingSessionCommandHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/TarotNow.Application.UnitTests.csproj` - Project test xac dinh dependency/framework cho bo test tuong ung.
- `Backend/tests/TarotNow.Application.UnitTests/Wallet/GetLedgerListQueryHandlerTests.cs` - Unit test 'GetLedgerListQueryHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Application.UnitTests/Wallet/GetWalletBalanceQueryHandlerTests.cs` - Unit test 'GetWalletBalanceQueryHandlerTests' xac nhan logic use-case/handler cua Application layer.
- `Backend/tests/TarotNow.Domain.UnitTests/TarotNow.Domain.UnitTests.csproj` - Project test xac dinh dependency/framework cho bo test tuong ung.
- `Backend/tests/TarotNow.Infrastructure.IntegrationTests/Security/Argon2idPasswordHasherTests.cs` - Integration test 'Argon2idPasswordHasherTests' kiem chung implementation ha tang voi thu vien/crypto/provider that.
- `Backend/tests/TarotNow.Infrastructure.IntegrationTests/TarotNow.Infrastructure.IntegrationTests.csproj` - Project test xac dinh dependency/framework cho bo test tuong ung.
- `Backend/tests/TarotNow.Infrastructure.UnitTests/BackgroundJobs/EscrowTimerServiceTests.cs` - Unit test 'EscrowTimerServiceTests' cho service/job o Infrastructure layer.
- `Backend/tests/TarotNow.Infrastructure.UnitTests/Rng/RngServiceTests.cs` - Unit test 'RngServiceTests' cho service/job o Infrastructure layer.
- `Backend/tests/TarotNow.Infrastructure.UnitTests/TarotNow.Infrastructure.UnitTests.csproj` - Project test xac dinh dependency/framework cho bo test tuong ung.

## Frontend
- `Frontend/.github/workflows/playwright.yml` - Workflow CI chay Playwright E2E de bat loi giao dien/luong nguoi dung.
- `Frontend/.gitignore` - Danh sach file/thu muc Frontend duoc bo qua khi commit.
- `Frontend/README.md` - Tai lieu huong dan setup/chay Frontend cho dev moi.
- `Frontend/eslint.config.mjs` - Quy tac lint JavaScript/TypeScript de giu chat luong code Frontend.
- `Frontend/messages/en.json` - Thong diep i18n tieng Anh cho UI theo namespace next-intl.
- `Frontend/messages/vi.json` - Thong diep i18n tieng Viet cho UI theo namespace next-intl.
- `Frontend/messages/zh.json` - Thong diep i18n tieng Trung cho UI theo namespace next-intl.
- `Frontend/next.config.ts` - Cau hinh Next.js (build/runtime/headers/experimental options).
- `Frontend/package-lock.json` - Khoa phien ban dependency npm de cai dat tai lap on dinh.
- `Frontend/package.json` - Khai bao script, dependency va thong tin package cua Frontend.
- `Frontend/playwright.config.ts` - Cau hinh Playwright: project browser, baseURL, retry, reporter.
- `Frontend/postcss.config.mjs` - Cau hinh PostCSS pipeline (vi du Tailwind/autoprefixer).
- `Frontend/public/file.svg` - Tai nguyen SVG 'file' dung cho icon/minh hoa giao dien.
- `Frontend/public/globe.svg` - Tai nguyen SVG 'globe' dung cho icon/minh hoa giao dien.
- `Frontend/public/locales/en/common.json` - Tu dien locale EN dung cho cac text chung o client.
- `Frontend/public/locales/vi/common.json` - Tu dien locale VI dung cho cac text chung o client.
- `Frontend/public/next.svg` - Tai nguyen SVG 'next' dung cho icon/minh hoa giao dien.
- `Frontend/public/vercel.svg` - Tai nguyen SVG 'vercel' dung cho icon/minh hoa giao dien.
- `Frontend/public/window.svg` - Tai nguyen SVG 'window' dung cho icon/minh hoa giao dien.
- `Frontend/src/actions/adminActions.ts` - Action module 'admin': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/authActions.ts` - Action module 'auth': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/chatActions.ts` - Action module 'chat': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/collectionActions.ts` - Action module 'collection': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/depositActions.ts` - Action module 'deposit': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/escrowActions.ts` - Action module 'escrow': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/historyActions.ts` - Action module 'history': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/legalActions.ts` - Action module 'legal': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/mfaActions.ts` - Action module 'mfa': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/profileActions.ts` - Action module 'profile': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/promotionActions.ts` - Action module 'promotion': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/readerActions.ts` - Action module 'reader': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/readingActions.ts` - Action module 'reading': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/walletActions.ts` - Action module 'wallet': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/actions/withdrawalActions.ts` - Action module 'withdrawal': dong goi ham goi API va xu ly nghiep vu phia Frontend theo domain.
- `Frontend/src/app/[locale]/(auth)/forgot-password/page.tsx` - Trang gui yeu cau quen mat khau.
- `Frontend/src/app/[locale]/(auth)/login/page.tsx` - Trang dang nhap nguoi dung.
- `Frontend/src/app/[locale]/(auth)/register/page.tsx` - Trang dang ky tai khoan moi.
- `Frontend/src/app/[locale]/(auth)/reset-password/page.tsx` - Trang dat lai mat khau bang token/OTP.
- `Frontend/src/app/[locale]/(auth)/verify-email/page.tsx` - Trang xac thuc email sau dang ky.
- `Frontend/src/app/[locale]/(user)/chat/[id]/page.tsx` - Trang chi tiet mot hoi thoai chat theo id.
- `Frontend/src/app/[locale]/(user)/chat/page.tsx` - Trang danh sach hoi thoai chat cua nguoi dung.
- `Frontend/src/app/[locale]/(user)/collection/page.tsx` - Trang bo suu tap la bai cua nguoi dung.
- `Frontend/src/app/[locale]/(user)/layout.tsx` - Layout cho toan bo nhom route user (menu, nav, provider dung chung).
- `Frontend/src/app/[locale]/(user)/loading.tsx` - Loading skeleton/fallback khi cac trang user dang tai du lieu.
- `Frontend/src/app/[locale]/(user)/profile/mfa/page.tsx` - Trang quan ly MFA trong profile.
- `Frontend/src/app/[locale]/(user)/profile/page.tsx` - Trang thong tin ca nhan va cai dat tai khoan.
- `Frontend/src/app/[locale]/(user)/profile/reader/page.tsx` - Trang cap nhat thong tin reader profile.
- `Frontend/src/app/[locale]/(user)/reader/apply/page.tsx` - Trang nop don dang ky tro thanh reader.
- `Frontend/src/app/[locale]/(user)/readers/[id]/page.tsx` - Trang xem chi tiet ho so reader.
- `Frontend/src/app/[locale]/(user)/readers/page.tsx` - Trang liet ke reader de user lua chon.
- `Frontend/src/app/[locale]/(user)/reading/history/[id]/page.tsx` - Trang chi tiet mot ket qua doc bai trong lich su.
- `Frontend/src/app/[locale]/(user)/reading/history/page.tsx` - Trang lich su cac phien doc bai.
- `Frontend/src/app/[locale]/(user)/reading/page.tsx` - Trang thao tac doc bai tarot cho user.
- `Frontend/src/app/[locale]/(user)/wallet/deposit/page.tsx` - Trang tao/giam sat giao dich nap tien vao vi.
- `Frontend/src/app/[locale]/(user)/wallet/page.tsx` - Trang tong quan vi (so du, bien dong).
- `Frontend/src/app/[locale]/(user)/wallet/withdraw/page.tsx` - Trang tao yeu cau rut tien tu vi.
- `Frontend/src/app/[locale]/admin/deposits/page.tsx` - Trang admin quan ly/duyet cac lenh nap tien.
- `Frontend/src/app/[locale]/admin/disputes/page.tsx` - Trang admin xu ly tranh chap escrow/chat.
- `Frontend/src/app/[locale]/admin/layout.tsx` - Layout rieng cho khu admin, bao gom bao ve va khung dieu huong quan tri.
- `Frontend/src/app/[locale]/admin/page.tsx` - Trang khung dashboard/admin area.
- `Frontend/src/app/[locale]/admin/promotions/page.tsx` - Trang admin quan ly chuong trinh khuyen mai.
- `Frontend/src/app/[locale]/admin/promotions/promotions-client.tsx` - Client component quan ly UI thao tac khuyen mai trong trang admin/promotions.
- `Frontend/src/app/[locale]/admin/reader-requests/page.tsx` - Trang admin duyet yeu cau dang ky reader.
- `Frontend/src/app/[locale]/admin/readings/page.tsx` - Trang admin theo doi du lieu phien doc bai.
- `Frontend/src/app/[locale]/admin/users/page.tsx` - Trang admin quan tri nguoi dung va trang thai tai khoan.
- `Frontend/src/app/[locale]/admin/withdrawals/page.tsx` - Trang admin quan ly/duyet yeu cau rut tien.
- `Frontend/src/app/[locale]/globals.css` - CSS global cho nhom route theo locale.
- `Frontend/src/app/[locale]/layout.tsx` - Layout goc theo locale: provider, metadata, khung dung chung.
- `Frontend/src/app/[locale]/legal/ai-disclaimer/page.tsx` - Trang dieu khoan mien tru lien quan AI interpretation.
- `Frontend/src/app/[locale]/legal/privacy/page.tsx` - Trang chinh sach bao mat.
- `Frontend/src/app/[locale]/legal/tos/page.tsx` - Trang dieu khoan su dung dich vu.
- `Frontend/src/app/[locale]/loading.tsx` - UI loading fallback khi route locale dang tai.
- `Frontend/src/app/[locale]/page.tsx` - Trang home theo locale.
- `Frontend/src/app/[locale]/reading/session/[id]/page.tsx` - Trang hien thi phien doc bai theo session id.
- `Frontend/src/app/favicon.ico` - Icon hien thi tren tab trinh duyet cua ung dung.
- `Frontend/src/app/globals.css` - CSS global cap app root (khong phu thuoc locale).
- `Frontend/src/components/AiInterpretationStream.tsx` - Component hien thi dong noi dung AI interpretation theo dang stream realtime.
- `Frontend/src/components/auth/AuthSessionManager.tsx` - Component quan ly session dang nhap, token state va dong bo auth tren client.
- `Frontend/src/components/auth/MfaChallengeModal.tsx` - Modal thu thap/verify ma MFA khi can challenge.
- `Frontend/src/components/chat/DisputeButton.tsx` - Nut khoi tao tranh chap cho luong chat/escrow.
- `Frontend/src/components/chat/EscrowPanel.tsx` - Panel hien thi trang thai escrow va thao tac lien quan trong chat.
- `Frontend/src/components/chat/ReportModal.tsx` - Modal gui bao cao vi pham/noi dung khong phu hop.
- `Frontend/src/components/common/LanguageSwitcher.tsx` - Component chuyen ngon ngu/locale giao dien.
- `Frontend/src/components/common/Navbar.tsx` - Thanh dieu huong chinh cua ung dung.
- `Frontend/src/components/common/ThemeSwitcher.tsx` - Component doi theme giao dien (light/dark hoac bien the).
- `Frontend/src/components/common/WalletWidget.tsx` - Widget hien thi nhanh thong tin vi va so du.
- `Frontend/src/components/layout/AstralBackground.tsx` - Nen/trang tri layout tao khong gian visual chu de tarot.
- `Frontend/src/components/layout/AuthLayout.tsx` - Layout dung rieng cho nhom trang xac thuc.
- `Frontend/src/components/layout/BottomTabBar.tsx` - Thanh dieu huong duoi (mobile-first) cho khu user.
- `Frontend/src/components/layout/Footer.tsx` - Chan trang dung chung toan bo site.
- `Frontend/src/components/layout/RoutePrefetcher.tsx` - Component prefetch route de cai thien toc do dieu huong.
- `Frontend/src/components/layout/UserLayout.tsx` - Layout khung cho khu vuc user da dang nhap.
- `Frontend/src/components/layout/UserSidebar.tsx` - Sidebar menu cho dashboard user.
- `Frontend/src/components/ui/Badge.tsx` - UI primitive 'Badge' dung lai nhieu noi de giu tinh nhat quan giao dien.
- `Frontend/src/components/ui/Button.tsx` - UI primitive 'Button' dung lai nhieu noi de giu tinh nhat quan giao dien.
- `Frontend/src/components/ui/EmptyState.tsx` - UI primitive 'EmptyState' dung lai nhieu noi de giu tinh nhat quan giao dien.
- `Frontend/src/components/ui/GlassCard.tsx` - UI primitive 'GlassCard' dung lai nhieu noi de giu tinh nhat quan giao dien.
- `Frontend/src/components/ui/Input.tsx` - UI primitive 'Input' dung lai nhieu noi de giu tinh nhat quan giao dien.
- `Frontend/src/components/ui/LoadingSpinner.tsx` - UI primitive 'LoadingSpinner' dung lai nhieu noi de giu tinh nhat quan giao dien.
- `Frontend/src/components/ui/Modal.tsx` - UI primitive 'Modal' dung lai nhieu noi de giu tinh nhat quan giao dien.
- `Frontend/src/components/ui/Pagination.tsx` - UI primitive 'Pagination' dung lai nhieu noi de giu tinh nhat quan giao dien.
- `Frontend/src/components/ui/SectionHeader.tsx` - UI primitive 'SectionHeader' dung lai nhieu noi de giu tinh nhat quan giao dien.
- `Frontend/src/components/ui/SkeletonLoader.tsx` - UI primitive 'SkeletonLoader' dung lai nhieu noi de giu tinh nhat quan giao dien.
- `Frontend/src/components/ui/index.ts` - Barrel export cho bo UI components de import gon va de quan ly.
- `Frontend/src/i18n/request.ts` - Khoi tao request config cho i18n (detect locale, load messages).
- `Frontend/src/i18n/routing.ts` - Khai bao routing theo locale va helper dieu huong da ngon ngu.
- `Frontend/src/lib/api.ts` - HTTP client wrapper goi Backend API (headers, token, error handling).
- `Frontend/src/lib/auth-client.ts` - Tien ich auth phia client: luu token, refresh, state helper.
- `Frontend/src/lib/jwt.ts` - Tien ich parse/kiem tra JWT tren Frontend.
- `Frontend/src/lib/tarotData.ts` - Du lieu/metadata bo bai tarot phuc vu render va logic client.
- `Frontend/src/proxy.ts` - Proxy helper/bridge de gui request API va xu ly base endpoint an toan.
- `Frontend/src/store/authStore.ts` - Store quan ly trang thai xac thuc tren client.
- `Frontend/src/store/walletStore.ts` - Store quan ly trang thai vi/so du va cap nhat realtime.
- `Frontend/src/types/auth.ts` - Kieu TypeScript cho domain 'auth' giup type-safe khi goi API/render UI.
- `Frontend/src/types/chat.ts` - Kieu TypeScript cho domain 'chat' giup type-safe khi goi API/render UI.
- `Frontend/src/types/escrow.ts` - Kieu TypeScript cho domain 'escrow' giup type-safe khi goi API/render UI.
- `Frontend/src/types/mfa.ts` - Kieu TypeScript cho domain 'mfa' giup type-safe khi goi API/render UI.
- `Frontend/src/types/reader.ts` - Kieu TypeScript cho domain 'reader' giup type-safe khi goi API/render UI.
- `Frontend/src/types/wallet.ts` - Kieu TypeScript cho domain 'wallet' giup type-safe khi goi API/render UI.
- `Frontend/src/types/withdrawal.ts` - Kieu TypeScript cho domain 'withdrawal' giup type-safe khi goi API/render UI.
- `Frontend/tests/example.spec.ts` - Playwright test mau, dung lam khung viet E2E test moi.
- `Frontend/tests/viewport-qa.spec.ts` - Playwright test kiem tra hien thi tren nhieu viewport/man hinh.
- `Frontend/tsconfig.json` - Cau hinh TypeScript compiler cho Frontend.
