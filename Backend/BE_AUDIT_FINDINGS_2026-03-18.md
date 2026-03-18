# Backend Audit Report (BE) - Current Code State - 2026-03-18

## 1) Pham vi va cach kiem tra
- Pham vi: `Backend/src/*`, `Backend/tests/*`, `database/postgresql/schema.sql`.
- Kiem tra thu cong theo risk pattern: logic tai chinh, idempotency, race condition, consistency migration/schema, cau hinh van hanh.
- Build/test tai thoi diem audit:
  - `dotnet build Backend/TarotNow.slnx` -> pass (0 warning, 0 error)
  - `dotnet test Backend/TarotNow.slnx --no-build` -> pass tat ca test projects

## 2) Tong ket nhanh
- Critical: 1
- High: 4
- Medium: 4
- Low: 3
- Tong cong: 12 van de

## 3) Danh sach loi chi tiet

### [CRITICAL][BE-01] Client disconnect sau khi da freeze co the de lai frozen diamond vo thoi han
- Mo ta:
  - `CompleteAiStreamCommandHandler` cap nhat status truoc, sau do chi `RefundAsync` neu `IsClientDisconnect == false`.
  - Nhanh `client disconnect` sau first token se vao status `failed_after_first_token` va bo qua ca refund lan consume.
  - Ket qua: `FrozenDiamondBalance` khong duoc giai quyet.
- Bang chung:
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommand.cs:119`
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommand.cs:122`
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommand.cs:146`
  - `Backend/src/TarotNow.Api/Controllers/AiController.cs:148`
  - `Backend/src/TarotNow.Api/Controllers/AiController.cs:171`
- Anh huong:
  - Sai lech so du vi, gay ket tien khong thu hoi duoc, de phat sinh khiu nai tai chinh.
- Huong xu ly de xuat:
  - Chuan hoa settlement cho disconnect: hoac consume (neu da co token), hoac refund co dieu kien ro rang.
  - Dam bao moi trang thai ket thuc deu ket thuc escrow (khong de dangling frozen).

### [HIGH][BE-02] Gioi han `finish_reason` (50 ky tu) co the lam fail update va chan settlement tiep theo
- Mo ta:
  - Handler gan truc tiep `record.FinishReason = request.ErrorMessage` roi goi `UpdateAsync`.
  - Column `finish_reason` chi cho phep max 50.
  - Neu message dai (>50), DB update fail truoc khi vao logic refund/consume.
- Bang chung:
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommand.cs:82`
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommand.cs:97`
  - `Backend/src/TarotNow.Infrastructure/Persistence/Configurations/AiRequestConfiguration.cs:31`
- Anh huong:
  - Mat nhat quan trang thai + nguy co de lai frozen balance.
- Huong xu ly de xuat:
  - Cat/chuan hoa `ErrorMessage` truoc khi luu (vd 50 ky tu + ma loi no i bo).
  - Hoac mo rong schema va tach `public reason`/`internal details`.

### [HIGH][BE-03] Bat nhat ti gia Diamond giua deposit va withdrawal
- Mo ta:
  - Deposit dang tinh `10,000 VND = 1 Diamond`.
  - Withdrawal va domain docs dang tinh `1,000 VND = 1 Diamond`.
- Bang chung:
  - `Backend/src/TarotNow.Application/Features/Deposit/Commands/CreateDepositOrder/CreateDepositOrderCommandHandler.cs:22`
  - `Backend/src/TarotNow.Application/Features/Deposit/Commands/CreateDepositOrder/CreateDepositOrderCommandHandler.cs:23`
  - `Backend/src/TarotNow.Application/Features/Withdrawal/Commands/CreateWithdrawal/CreateWithdrawalCommand.cs:19`
  - `Backend/src/TarotNow.Application/Features/Withdrawal/Commands/CreateWithdrawal/CreateWithdrawalCommand.cs:93`
  - `Backend/src/TarotNow.Domain/Entities/UserWallet.cs:30`
- Anh huong:
  - Sai lech kinh te he thong, nguy co arbitrage va bug tinh phi/hoan tien.
- Huong xu ly de xuat:
  - Dua FX ratio ve 1 nguon cau hinh duy nhat (SystemConfig/Options) va dung chung cho deposit, withdrawal, report.

### [HIGH][BE-04] Escrow co nguy co race/lost update o ReaderReply va OpenDispute
- Mo ta:
  - `ReaderReply` va `OpenDispute` doc item qua `GetItemByIdAsync` (khong lock), update metadata roi `SaveChanges`.
  - Cac luong timer auto-refund/auto-release lai dung `FOR UPDATE` + transaction.
  - Co the xay ra tranh chap ghi de status/timer giua request nguoi dung va background job.
- Bang chung:
  - `Backend/src/TarotNow.Application/Features/Escrow/Commands/ReaderReply/ReaderReplyCommand.cs:29`
  - `Backend/src/TarotNow.Application/Features/Escrow/Commands/ReaderReply/ReaderReplyCommand.cs:50`
  - `Backend/src/TarotNow.Application/Features/Escrow/Commands/OpenDispute/OpenDisputeCommand.cs:34`
  - `Backend/src/TarotNow.Application/Features/Escrow/Commands/OpenDispute/OpenDisputeCommand.cs:61`
  - `Backend/src/TarotNow.Infrastructure/Repositories/ChatFinanceRepository.cs:56`
  - `Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.cs:111`
  - `Backend/src/TarotNow.Infrastructure/BackgroundJobs/EscrowTimerService.cs:171`
- Anh huong:
  - Trang thai escrow khong on dinh, co the sai tien freeze/session metadata.
- Huong xu ly de xuat:
  - Dong bo pattern settlement: cac transition status nhay cam deu chay trong transaction + row lock (`GetItemForUpdateAsync`).

### [HIGH][BE-05] Drift migration/schema lam thieu rang buoc unique cho escrow idempotency
- Mo ta:
  - Logic `AcceptOffer`/`AddQuestion` dua vao idempotency key theo mau check-then-insert.
  - Migration EF hien tai khong tao unique index cho:
    - `chat_question_items.idempotency_key`
    - `chat_finance_sessions.conversation_ref`
  - `schema.sql` lai co rang buoc unique cho cac cot nay.
- Bang chung:
  - `Backend/src/TarotNow.Application/Features/Escrow/Commands/AcceptOffer/AcceptOfferCommand.cs:49`
  - `Backend/src/TarotNow.Application/Features/Escrow/Commands/AddQuestion/AddQuestionCommand.cs:48`
  - `Backend/src/TarotNow.Infrastructure/Migrations/20260317211223_AlignSchemaWithMongoAndSecurity.cs:50`
  - `Backend/src/TarotNow.Infrastructure/Migrations/20260317211223_AlignSchemaWithMongoAndSecurity.cs:94`
  - `Backend/src/TarotNow.Infrastructure/Migrations/ApplicationDbContextModelSnapshot.cs:156`
  - `Backend/src/TarotNow.Infrastructure/Migrations/ApplicationDbContextModelSnapshot.cs:240`
  - `database/postgresql/schema.sql:440`
  - `database/postgresql/schema.sql:495`
- Anh huong:
  - Neu deploy bang EF migrations, race request dong thoi co the tao duplicate session/item.
- Huong xu ly de xuat:
  - Bo sung unique indexes trong EF model + migration moi.
  - Catch unique-violation de tra ve idempotent response thay vi 500.

### [MEDIUM][BE-06] `CheckConsent` hard-code version `1.0` khi check tung document
- Mo ta:
  - Neu truyen `DocumentType` nhung khong truyen `Version`, code mac dinh `1.0` thay vi lay version hien hanh tu config.
- Bang chung:
  - `Backend/src/TarotNow.Application/Features/Legal/Queries/CheckConsent/CheckConsentQueryHandler.cs:35`
- Anh huong:
  - Sau khi bump version legal, API co the bao "da consent" sai.
- Huong xu ly de xuat:
  - Map `DocumentType` -> version tu `LegalSettings` thay vi hard-code.

### [MEDIUM][BE-07] `HasConsented` duoc su dung trong app nhung bi EF ignore
- Mo ta:
  - `UserConfiguration` ignore `HasConsented`.
  - `GetProfile` van tra ve field nay, `RecordConsent` van cap nhat field nay.
  - Gia tri de mat tinh ben vung (re-hydrate tu DB se ve default false).
- Bang chung:
  - `Backend/src/TarotNow.Infrastructure/Persistence/Configurations/UserConfiguration.cs:51`
  - `Backend/src/TarotNow.Application/Features/Profile/Queries/GetProfile/GetProfileQueryHandler.cs:36`
  - `Backend/src/TarotNow.Application/Features/Legal/Commands/RecordConsent/RecordConsentCommandHandler.cs:43`
- Anh huong:
  - API profile co the tra ve thong tin sai ve trang thai consent.
- Huong xu ly de xuat:
  - Hoac bo field khoi User/Profile, hoac map va persist ro rang.
  - Uu tien dung `user_consents` lam single source of truth.

### [MEDIUM][BE-08] Cac nguong AI quota/rate-limit dang hard-code, khong dung `SystemConfig`
- Mo ta:
  - `DailyAiQuota`, `InFlightAiCap`, `ReadingRateLimitSeconds` da co trong appsettings.
  - Handler van hard-code 3/3/30 giay.
- Bang chung:
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommand.cs:61`
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommand.cs:65`
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommand.cs:74`
  - `Backend/src/TarotNow.Api/appsettings.json:64`
- Anh huong:
  - Kho van hanh (ops) va de drift giua moi truong.
- Huong xu ly de xuat:
  - Bind options (`IOptions<SystemConfig>`) va dung 1 nguon config.

### [MEDIUM][BE-09] Duplicate withdrawal theo race co the tra ve 500 thay vi loi nghiep vu
- Mo ta:
  - Co pre-check + re-check, nhung van co cua so race tai luc insert/commit.
  - Unique index co the nem `DbUpdateException`, nhung global handler khong map loai loi nay.
- Bang chung:
  - `Backend/src/TarotNow.Application/Features/Withdrawal/Commands/CreateWithdrawal/CreateWithdrawalCommand.cs:82`
  - `Backend/src/TarotNow.Application/Features/Withdrawal/Commands/CreateWithdrawal/CreateWithdrawalCommand.cs:109`
  - `Backend/src/TarotNow.Application/Features/Withdrawal/Commands/CreateWithdrawal/CreateWithdrawalCommand.cs:128`
  - `Backend/src/TarotNow.Infrastructure/Persistence/Configurations/WithdrawalRequestConfiguration.cs:11`
  - `Backend/src/TarotNow.Api/Middlewares/GlobalExceptionHandler.cs:39`
- Anh huong:
  - Trai nghiem API khong on dinh (500 thay vi 400 "da tao yeu cau hom nay").
- Huong xu ly de xuat:
  - Catch unique violation (Postgres `23505`) va map sang `BadRequestException`/409.

### [LOW][BE-10] Trang thai `first_token_received` duoc khai bao nhung khong thay duoc set
- Mo ta:
  - Enum co `FirstTokenReceived`.
  - Counter active/daily tinh theo status nay.
  - Luong stream hien tai chi set `requested`, `completed`, `failed_*`.
- Bang chung:
  - `Backend/src/TarotNow.Domain/Enums/AiRequestStatus.cs:9`
  - `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/AiRequestRepository.cs:45`
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/StreamReading/StreamReadingCommand.cs:106`
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommand.cs:81`
- Anh huong:
  - State machine kho hieu, de sai khi bao tri/bao cao.
- Huong xu ly de xuat:
  - Hoac bo status khoi model, hoac bo sung transition ro rang khi nhan token dau tien.

### [LOW][BE-11] `RetryCount = -1` rollback quota la logic thua/khong con tac dung
- Mo ta:
  - Code set sentinel `RetryCount = -1` de exclude khoi daily count.
  - Daily count hien tai khong su dung `RetryCount`, ma chi loc theo `status`.
- Bang chung:
  - `Backend/src/TarotNow.Application/Features/Reading/Commands/CompleteAiStream/CompleteAiStreamCommand.cs:143`
  - `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/AiRequestRepository.cs:39`
- Anh huong:
  - Dead logic + gay hieu nham cho dev ve cach rollback quota.
- Huong xu ly de xuat:
  - Xoa sentinel hoac cap nhat bo dem quota theo thiet ke moi.

### [LOW][BE-12] Method repository du khong duoc contract hoa va khong thay call-site
- Mo ta:
  - `AiRequestRepository` co `GetByIdempotencyKeyAsync` nhung interface khong dinh nghia.
  - Rasoat call-site hien tai khong thay dung method nay.
- Bang chung:
  - `Backend/src/TarotNow.Infrastructure/Persistence/Repositories/AiRequestRepository.cs:22`
  - `Backend/src/TarotNow.Application/Interfaces/IAiRequestRepository.cs:9`
- Anh huong:
  - Tang no ky thuat, de gay nham lan khi maintain.
- Huong xu ly de xuat:
  - Hoac dua vao interface va su dung ro rang, hoac xoa neu khong can.

## 4) Ghi chu quan trong
- Build/test pass KHONG loai tru duoc cac loi logic/race condition o tren.
- Uu tien fix ngay nhom tai chinh-escrow-AI settlement: BE-01, BE-02, BE-04, BE-05.
