# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T16:58:30.517Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 13
- High pages (request count): 147
- High slow requests: 113
- Medium slow requests: 348

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 3346 | 225 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 3287 | 1275 | 2 | 0 | 17 | 0 | yes |
| logged-in-reader | desktop | 33 | 3324 | 968 | 3 | 0 | 15 | 0 | yes |
| logged-out | mobile | 9 | 3262 | 226 | 0 | 0 | 0 | 1 | yes |
| logged-in-admin | mobile | 45 | 3156 | 1388 | 5 | 0 | 12 | 0 | yes |
| logged-in-reader | mobile | 35 | 3278 | 1084 | 1 | 0 | 19 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.8 | 3414 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2789 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6693 | 0 |
| logged-in-admin | desktop | community | 1 | 36.0 | 4085 | 0 |
| logged-in-admin | desktop | gacha | 2 | 33.5 | 3622 | 0 |
| logged-in-admin | desktop | gamification | 1 | 31.0 | 3764 | 0 |
| logged-in-admin | desktop | home | 1 | 35.0 | 3459 | 0 |
| logged-in-admin | desktop | inventory | 1 | 37.0 | 3179 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3213 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2939 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2693 | 0 |
| logged-in-admin | desktop | profile | 3 | 28.3 | 3411 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2716 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.7 | 2937 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.8 | 3205 | 0 |
| logged-in-admin | desktop | wallet | 4 | 29.3 | 2931 | 2 |
| logged-in-admin | mobile | admin | 10 | 29.6 | 3066 | 0 |
| logged-in-admin | mobile | auth | 2 | 51.5 | 3570 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2733 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5635 | 0 |
| logged-in-admin | mobile | community | 1 | 30.0 | 3457 | 0 |
| logged-in-admin | mobile | gacha | 2 | 34.5 | 3210 | 0 |
| logged-in-admin | mobile | gamification | 1 | 58.0 | 4867 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2733 | 0 |
| logged-in-admin | mobile | inventory | 1 | 38.0 | 3046 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 2707 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2974 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 3368 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.3 | 2888 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2803 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2953 | 0 |
| logged-in-admin | mobile | reading | 5 | 30.6 | 3213 | 0 |
| logged-in-admin | mobile | wallet | 4 | 27.8 | 3037 | 5 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2762 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6847 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3430 | 0 |
| logged-in-reader | desktop | gacha | 2 | 33.5 | 3341 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 3105 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2698 | 0 |
| logged-in-reader | desktop | inventory | 1 | 35.0 | 3244 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2792 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 3875 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2945 | 0 |
| logged-in-reader | desktop | profile | 3 | 31.3 | 3353 | 1 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2935 | 0 |
| logged-in-reader | desktop | readers | 7 | 29.1 | 3067 | 0 |
| logged-in-reader | desktop | reading | 5 | 30.4 | 3061 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.0 | 3445 | 2 |
| logged-in-reader | mobile | auth | 2 | 37.0 | 3332 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2767 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5654 | 0 |
| logged-in-reader | mobile | community | 1 | 37.0 | 3842 | 0 |
| logged-in-reader | mobile | gacha | 2 | 37.0 | 3376 | 0 |
| logged-in-reader | mobile | gamification | 1 | 30.0 | 2921 | 1 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2720 | 0 |
| logged-in-reader | mobile | inventory | 1 | 35.0 | 3207 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 35.0 | 4361 | 0 |
| logged-in-reader | mobile | legal | 3 | 26.3 | 2970 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2791 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.0 | 2957 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 3007 | 0 |
| logged-in-reader | mobile | readers | 7 | 29.0 | 3518 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.0 | 2923 | 0 |
| logged-in-reader | mobile | wallet | 4 | 36.5 | 3258 | 0 |
| logged-out | desktop | auth | 5 | 24.2 | 3400 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3971 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 3047 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 3075 | 0 |
| logged-out | mobile | home | 1 | 31.0 | 3386 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 3532 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3971 | 1444 | 1954 | 1460 | 1460 | 0.0000 | 170.0 | 601524 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2729 | 723 | 723 | 584 | 584 | 0.0000 | 0.0 | 512435 |
| logged-out | desktop | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4446 | 708 | 2440 | 600 | 600 | 0.0000 | 0.0 | 513690 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3226 | 688 | 1221 | 596 | 596 | 0.0000 | 0.0 | 512032 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3542 | 620 | 1531 | 564 | 564 | 0.0000 | 0.0 | 512070 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3059 | 632 | 1040 | 580 | 580 | 0.0000 | 0.0 | 512230 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3399 | 783 | 1390 | 628 | 628 | 0.0000 | 0.0 | 525834 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2745 | 734 | 734 | 592 | 592 | 0.0000 | 0.0 | 526129 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2996 | 712 | 972 | 572 | 572 | 0.0000 | 0.0 | 526041 |
| logged-in-admin | desktop | auth-public | /vi | 35 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3459 | 1138 | 1446 | 876 | 1528 | 0.0040 | 341.0 | 613395 |
| logged-in-admin | desktop | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3311 | 933 | 1304 | 652 | 1032 | 0.0042 | 0.0 | 645038 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 37 | 4 | critical | 0 | 0 | 1 | 0 | 5 | 2 | 3 | 5 | 0 | 0 | 3179 | 828 | 1136 | 632 | 1028 | 0.0042 | 0.0 | 654647 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 35 | 6 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3493 | 852 | 1474 | 628 | 1076 | 0.0042 | 0.0 | 732550 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3750 | 882 | 1742 | 616 | 1080 | 0.0042 | 0.0 | 728814 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 41 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6693 | 685 | 696 | 592 | 592 | 0.0043 | 0.0 | 643480 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 29 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4184 | 874 | 2179 | 644 | 1076 | 0.0489 | 0.0 | 634241 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2683 | 666 | 671 | 588 | 952 | 0.0042 | 0.0 | 631711 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3367 | 886 | 977 | 812 | 1076 | 0.0489 | 0.0 | 630936 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2830 | 824 | 824 | 600 | 984 | 0.0042 | 0.0 | 634796 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2789 | 780 | 781 | 624 | 1028 | 0.0042 | 0.0 | 631910 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3213 | 682 | 1207 | 632 | 976 | 0.0180 | 0.0 | 650901 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 36 | 4 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 4085 | 784 | 1321 | 628 | 2032 | 0.0042 | 0.0 | 779135 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3764 | 837 | 1756 | 632 | 988 | 0.0279 | 0.0 | 644644 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2831 | 825 | 825 | 620 | 1080 | 0.0042 | 0.0 | 634025 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2749 | 726 | 740 | 640 | 1040 | 0.0042 | 0.0 | 631713 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 33 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3371 | 790 | 1363 | 604 | 1120 | 0.0042 | 0.0 | 645285 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 37 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2773 | 887 | 1579 | 676 | 1060 | 0.0042 | 0.0 | 631276 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2693 | 675 | 686 | 632 | 1044 | 0.0047 | 0.0 | 632180 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2716 | 699 | 710 | 592 | 996 | 0.0042 | 0.0 | 632635 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2744 | 717 | 730 | 608 | 1224 | 0.0042 | 0.0 | 632889 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2701 | 694 | 694 | 600 | 908 | 0.0020 | 0.0 | 526255 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3000 | 766 | 993 | 620 | 984 | 0.0020 | 0.0 | 526461 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3117 | 635 | 1111 | 580 | 908 | 0.0020 | 0.0 | 526466 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3194 | 713 | 1182 | 632 | 984 | 0.0000 | 0.0 | 647686 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2909 | 831 | 898 | 600 | 1036 | 0.0000 | 0.0 | 647744 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 31 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4903 | 741 | 2896 | 660 | 1000 | 0.0000 | 0.0 | 649209 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3248 | 721 | 1242 | 684 | 1000 | 0.0022 | 0.0 | 699581 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2726 | 709 | 720 | 612 | 1000 | 0.0000 | 0.0 | 644763 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 31 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4480 | 814 | 2475 | 728 | 1076 | 0.0000 | 0.0 | 649376 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3704 | 800 | 1692 | 652 | 1008 | 0.0000 | 0.0 | 649118 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2810 | 785 | 786 | 648 | 1076 | 0.0000 | 0.0 | 656050 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3242 | 742 | 1074 | 628 | 1016 | 0.0000 | 0.0 | 650298 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 30 | 2 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2928 | 766 | 898 | 732 | 1000 | 0.0000 | 0.0 | 647170 |
| logged-in-admin | desktop | reading | /vi/reading/session/d3b73fcf-fe77-4fce-88b0-452d93bbfa7b | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2816 | 775 | 791 | 672 | 1132 | 0.0042 | 2.0 | 632496 |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | 31 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4089 | 1299 | 2044 | 644 | 1356 | 0.0042 | 0.0 | 713107 |
| logged-in-admin | desktop | reading | /vi/reading/session/2a0a3080-de77-4852-9a58-b3d1e92fff2f | 31 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3064 | 742 | 1042 | 636 | 1016 | 0.0042 | 0.0 | 713349 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 734 | 738 | 620 | 1024 | 0.0042 | 0.0 | 630957 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2815 | 796 | 805 | 612 | 1008 | 0.0042 | 0.0 | 631399 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 790 | 804 | 644 | 1056 | 0.0042 | 0.0 | 631377 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3294 | 711 | 1274 | 652 | 1224 | 0.0042 | 0.0 | 635943 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2996 | 893 | 983 | 632 | 1260 | 0.0042 | 0.0 | 633949 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3060 | 909 | 1038 | 724 | 1296 | 0.0042 | 0.0 | 633175 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2698 | 690 | 691 | 656 | 1260 | 0.0033 | 252.0 | 537822 |
| logged-in-reader | desktop | reading | /vi/reading | 32 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3408 | 829 | 1399 | 600 | 988 | 0.0039 | 0.0 | 653933 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 35 | 5 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3244 | 827 | 1236 | 628 | 1060 | 0.0039 | 0.0 | 652349 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 37 | 5 | critical | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3374 | 817 | 1365 | 596 | 996 | 0.0039 | 0.0 | 735356 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3308 | 831 | 1299 | 592 | 992 | 0.0039 | 0.0 | 724365 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 32 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6847 | 792 | 1146 | 612 | 960 | 0.0040 | 0.0 | 642289 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3050 | 875 | 1040 | 628 | 1044 | 0.0726 | 0.0 | 638862 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 33 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4032 | 701 | 2023 | 568 | 964 | 0.0039 | 0.0 | 644897 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2977 | 693 | 965 | 576 | 984 | 0.0039 | 0.0 | 632578 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2850 | 830 | 842 | 576 | 976 | 0.0039 | 0.0 | 635374 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2762 | 748 | 752 | 620 | 1004 | 0.0039 | 0.0 | 631681 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2792 | 776 | 780 | 620 | 972 | 0.0177 | 0.0 | 649813 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3430 | 691 | 699 | 628 | 1772 | 0.0039 | 0.0 | 643376 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3105 | 891 | 1096 | 620 | 1048 | 0.0903 | 0.0 | 642318 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 27 | 44 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3459 | 978 | 1072 | 924 | 1628 | 0.0038 | 330.0 | 607151 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2666 | 652 | 653 | 604 | 1016 | 0.0039 | 0.0 | 631844 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4741 | 694 | 2724 | 580 | 940 | 0.0039 | 0.0 | 631914 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2914 | 896 | 896 | 604 | 992 | 0.0095 | 0.0 | 634341 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2945 | 918 | 918 | 616 | 1036 | 0.0040 | 0.0 | 633536 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2935 | 781 | 891 | 1080 | 1080 | 0.0039 | 0.0 | 632792 |
| logged-in-reader | desktop | reading | /vi/reading/history | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2970 | 914 | 956 | 600 | 1144 | 0.0039 | 0.0 | 635455 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4612 | 748 | 2601 | 604 | 936 | 0.0019 | 0.0 | 526305 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2789 | 777 | 777 | 616 | 952 | 0.0019 | 0.0 | 526243 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4224 | 727 | 2199 | 636 | 940 | 0.0019 | 0.0 | 526436 |
| logged-in-reader | desktop | reading | /vi/reading/session/eacbcb32-5bb0-4271-bd9b-0c5345f67219 | 31 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3073 | 857 | 1053 | 976 | 976 | 0.0039 | 0.0 | 713012 |
| logged-in-reader | desktop | reading | /vi/reading/session/ba4ed496-3f1c-4bff-8ff1-4527fcb1cf44 | 31 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3087 | 743 | 1068 | 608 | 1012 | 0.0039 | 0.0 | 713175 |
| logged-in-reader | desktop | reading | /vi/reading/session/f7987eb3-81ef-4f1b-8d95-19a85bdd6aae | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 750 | 752 | 728 | 1160 | 0.0039 | 0.0 | 632425 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3574 | 809 | 1549 | 584 | 996 | 0.0039 | 0.0 | 631427 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2801 | 783 | 784 | 620 | 984 | 0.0039 | 0.0 | 631450 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3031 | 713 | 1019 | 600 | 1080 | 0.0039 | 0.0 | 631285 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3232 | 684 | 1212 | 616 | 1208 | 0.0039 | 0.0 | 636313 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2826 | 709 | 816 | 608 | 1180 | 0.0039 | 0.0 | 634278 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3157 | 811 | 1145 | 624 | 1216 | 0.0039 | 0.0 | 636531 |
| logged-out | mobile | auth-public | /vi | 31 | 2 | high | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 3386 | 1268 | 1268 | 1024 | 1024 | 0.0000 | 0.0 | 603572 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3466 | 653 | 1449 | 568 | 568 | 0.0000 | 0.0 | 512397 |
| logged-out | mobile | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3278 | 675 | 1254 | 588 | 588 | 0.0000 | 0.0 | 512880 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2656 | 612 | 645 | 540 | 540 | 0.0000 | 0.0 | 512039 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3331 | 753 | 1263 | 644 | 644 | 0.0000 | 0.0 | 512072 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2646 | 636 | 636 | 540 | 540 | 0.0000 | 0.0 | 512199 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3163 | 660 | 1152 | 564 | 564 | 0.0000 | 0.0 | 525982 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4080 | 716 | 2066 | 696 | 696 | 0.0000 | 0.0 | 525988 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3352 | 647 | 1341 | 556 | 556 | 0.0000 | 0.0 | 526075 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2733 | 723 | 724 | 548 | 896 | 0.0032 | 0.0 | 537754 |
| logged-in-admin | mobile | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2911 | 881 | 898 | 548 | 888 | 0.0000 | 0.0 | 645076 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 38 | 3 | critical | 0 | 0 | 2 | 0 | 5 | 0 | 5 | 5 | 0 | 0 | 3046 | 856 | 1028 | 540 | 884 | 0.0000 | 0.0 | 664874 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 37 | 5 | critical | 0 | 0 | 1 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 3195 | 858 | 1184 | 592 | 920 | 0.0071 | 0.0 | 799499 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3224 | 866 | 1211 | 572 | 912 | 0.0071 | 0.0 | 728753 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5635 | 706 | 707 | 572 | 572 | 0.0000 | 0.0 | 643251 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3074 | 775 | 1067 | 560 | 904 | 0.0000 | 0.0 | 637567 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2750 | 722 | 741 | 552 | 884 | 0.0000 | 0.0 | 631820 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 40 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 890 | 1014 | 560 | 900 | 0.0760 | 0.0 | 630976 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2872 | 862 | 862 | 540 | 868 | 0.0000 | 0.0 | 634135 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2733 | 722 | 722 | 572 | 896 | 0.0000 | 0.0 | 631660 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2707 | 694 | 696 | 532 | 864 | 0.0196 | 0.0 | 649543 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3457 | 720 | 720 | 544 | 1900 | 0.0051 | 0.0 | 643214 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 58 | 4 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4867 | 1142 | 2118 | 912 | 1276 | 0.0000 | 0.0 | 1206615 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 27 | 60 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2787 | 702 | - | 564 | 924 | 0.0000 | 0.0 | 607180 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3421 | 1271 | 1411 | 1276 | 1612 | 0.0000 | 0.0 | 631676 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3136 | 751 | 1129 | 568 | 908 | 0.0071 | 0.0 | 632531 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 40 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2805 | 881 | 881 | 656 | 1016 | 0.0071 | 0.0 | 631410 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3368 | 704 | 1359 | 528 | 864 | 0.0088 | 0.0 | 632126 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2803 | 785 | 794 | 552 | 876 | 0.0000 | 0.0 | 632510 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2990 | 974 | 981 | 560 | 936 | 0.0000 | 0.0 | 633016 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2685 | 675 | 675 | 548 | 856 | 0.0032 | 0.0 | 526344 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3518 | 747 | 1508 | 572 | 872 | 0.0055 | 0.0 | 526268 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2718 | 705 | 706 | 568 | 888 | 0.0032 | 0.0 | 526435 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3254 | 1223 | 1242 | 540 | 1156 | 0.0000 | 0.0 | 647483 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2776 | 761 | 762 | 556 | 880 | 0.0000 | 0.0 | 647916 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2818 | 792 | 807 | 572 | 880 | 0.0000 | 0.0 | 646065 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 33 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3873 | 727 | 1864 | 548 | 860 | 0.0000 | 0.0 | 699527 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2838 | 775 | 827 | 568 | 876 | 0.0000 | 0.0 | 644886 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2806 | 778 | 778 | 552 | 876 | 0.0000 | 0.0 | 646772 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3528 | 730 | 1514 | 580 | 908 | 0.0000 | 0.0 | 650576 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3226 | 857 | 1211 | 560 | 896 | 0.0000 | 0.0 | 689330 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 752 | 753 | 544 | 868 | 0.0000 | 0.0 | 649743 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2772 | 755 | 756 | 544 | 856 | 0.0000 | 0.0 | 646179 |
| logged-in-admin | mobile | reading | /vi/reading/session/33d0cbf1-f592-4e7f-a9ab-3513d4ab39f2 | 36 | 1 | critical | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4367 | 736 | 2358 | 548 | 4064 | 0.0071 | 0.0 | 704595 |
| logged-in-admin | mobile | reading | /vi/reading/session/87c68e71-1ef8-4aa9-8a53-7ca6a3e1a353 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2790 | 780 | 780 | 556 | 880 | 0.0000 | 0.0 | 632282 |
| logged-in-admin | mobile | reading | /vi/reading/session/6d3e854c-16d8-49dc-8b91-086f1421b9bc | 30 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3007 | 761 | 989 | 568 | 896 | 0.0000 | 0.0 | 680918 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2909 | 887 | 895 | 560 | 884 | 0.0000 | 0.0 | 631344 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2916 | 900 | 901 | 712 | 1028 | 0.0000 | 0.0 | 631211 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3042 | 971 | 1012 | 584 | 908 | 0.0000 | 0.0 | 631238 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2998 | 950 | 971 | 572 | 924 | 0.0000 | 0.0 | 634977 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2740 | 718 | 719 | 536 | 932 | 0.0000 | 0.0 | 632962 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3195 | 684 | 1184 | 540 | 888 | 0.0071 | 0.0 | 633179 |
| logged-in-admin | mobile | auth-public | /vi/login | 50 | 13 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3425 | 713 | 713 | 532 | 888 | 0.0055 | 0.0 | 1037676 |
| logged-in-admin | mobile | auth-public | /vi/register | 53 | 10 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3715 | 811 | 1017 | 540 | 904 | 0.0055 | 0.0 | 1108143 |
| logged-in-reader | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2720 | 710 | 710 | 544 | 904 | 0.0032 | 0.0 | 537954 |
| logged-in-reader | mobile | reading | /vi/reading | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2924 | 848 | 892 | 552 | 916 | 0.0000 | 0.0 | 642823 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 35 | 5 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3207 | 817 | 1196 | 600 | 940 | 0.0071 | 0.0 | 661536 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 41 | 1 | critical | 0 | 0 | 2 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 3695 | 829 | 1621 | 564 | 912 | 0.0071 | 0.0 | 812151 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3057 | 863 | 1039 | 552 | 884 | 0.0000 | 0.0 | 727349 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 32 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5654 | 737 | 763 | 564 | 564 | 0.0000 | 0.0 | 642211 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3097 | 862 | 1089 | 560 | 900 | 0.1024 | 0.0 | 636931 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2968 | 864 | 959 | 988 | 988 | 0.0000 | 0.0 | 632018 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2805 | 778 | 791 | 552 | 1284 | 0.0000 | 0.0 | 632522 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2832 | 823 | 823 | 552 | 876 | 0.0000 | 0.0 | 635167 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2767 | 739 | 752 | 676 | 1052 | 0.0000 | 0.0 | 631824 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 35 | 2 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4361 | 787 | 2349 | 584 | 912 | 0.0196 | 0.0 | 662930 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 37 | 3 | critical | 0 | 0 | 2 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3842 | 752 | 1102 | 572 | 1680 | 0.0051 | 0.0 | 779718 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 30 | 8 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2921 | 861 | 909 | 560 | 884 | 0.0000 | 0.0 | 643200 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 56 | 6 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3739 | 1031 | 1031 | 872 | 1256 | 0.0000 | 0.0 | 1135425 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3499 | 1087 | 1487 | 920 | 1256 | 0.0000 | 0.0 | 635008 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2951 | 745 | 940 | 556 | 892 | 0.0000 | 0.0 | 635170 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2841 | 825 | 825 | 564 | 920 | 0.0330 | 0.0 | 633412 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2791 | 771 | 774 | 552 | 884 | 0.0000 | 0.0 | 632376 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3007 | 774 | 993 | 668 | 1000 | 0.0000 | 0.0 | 632637 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2756 | 725 | 741 | 572 | 924 | 0.0000 | 0.0 | 633127 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2702 | 685 | 685 | 536 | 844 | 0.0032 | 0.0 | 526348 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2940 | 664 | 928 | 536 | 844 | 0.0032 | 0.0 | 528059 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3269 | 766 | 1249 | 652 | 956 | 0.0032 | 0.0 | 528201 |
| logged-in-reader | mobile | reading | /vi/reading/session/ac436e4f-fad9-4cdc-bb04-edfbbb70f7dd | 30 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3042 | 699 | 1031 | 576 | 912 | 0.0071 | 0.0 | 681178 |
| logged-in-reader | mobile | reading | /vi/reading/session/6f456328-dcf6-424c-825e-9385d76fc19d | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2858 | 829 | 844 | 636 | 964 | 0.0000 | 0.0 | 632397 |
| logged-in-reader | mobile | reading | /vi/reading/session/306aa5d9-2a5c-4068-9fb0-74d97a4a410a | 30 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3037 | 754 | 1026 | 556 | 892 | 0.0071 | 0.0 | 681164 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2775 | 752 | 763 | 568 | 900 | 0.0000 | 0.0 | 631180 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3317 | 753 | 1298 | 564 | 908 | 0.0071 | 0.0 | 631494 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3046 | 755 | 1033 | 576 | 896 | 0.0071 | 0.0 | 631472 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 34 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6705 | 752 | 4693 | 644 | 980 | 0.0000 | 0.0 | 646704 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3177 | 731 | 1160 | 560 | 900 | 0.0071 | 0.0 | 633047 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2777 | 746 | 763 | 564 | 904 | 0.0000 | 0.0 | 633478 |
| logged-in-reader | mobile | auth-public | /vi/login | 50 | 13 | critical | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3438 | 750 | 750 | 556 | 928 | 0.0055 | 0.0 | 1037989 |
| logged-in-reader | mobile | auth-public | /vi/register | 24 | 41 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3225 | 711 | 711 | 548 | 912 | 0.0055 | 0.0 | 510964 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | desktop | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |
| logged-in-reader | mobile | /vi/collection | 0 | 0 | 0 | 0 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 35 | high | 5 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 37 | critical | 2 | 33 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 35 | high | 0 | 33 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 33 | high | 4 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/d3b73fcf-fe77-4fce-88b0-452d93bbfa7b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/2a0a3080-de77-4852-9a58-b3d1e92fff2f | 31 | high | 0 | 29 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | high | 3 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 32 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 37 | critical | 2 | 33 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 33 | high | 4 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 27 | high | 0 | 25 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/eacbcb32-5bb0-4271-bd9b-0c5345f67219 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/ba4ed496-3f1c-4bff-8ff1-4527fcb1cf44 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/f7987eb3-81ef-4f1b-8d95-19a85bdd6aae | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 31 | high | 3 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 31 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 38 | critical | 3 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 37 | critical | 2 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 32 | high | 3 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 58 | critical | 3 | 52 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 27 | high | 0 | 25 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 33 | high | 1 | 30 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/33d0cbf1-f592-4e7f-a9ab-3513d4ab39f2 | 36 | critical | 4 | 30 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/87c68e71-1ef8-4aa9-8a53-7ca6a3e1a353 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/6d3e854c-16d8-49dc-8b91-086f1421b9bc | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | auth-public | /vi/login | 50 | critical | 0 | 46 | 0 |
| logged-in-admin | mobile | auth-public | /vi/register | 53 | critical | 0 | 49 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 41 | critical | 5 | 34 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 33 | high | 3 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 35 | high | 4 | 29 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 37 | critical | 3 | 32 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 56 | critical | 3 | 49 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 27 | high | 1 | 23 | 0 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 27 | high | 1 | 23 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/ac436e4f-fad9-4cdc-bb04-edfbbb70f7dd | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/6f456328-dcf6-424c-825e-9385d76fc19d | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/306aa5d9-2a5c-4068-9fb0-74d97a4a410a | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 34 | high | 5 | 27 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi/login | 50 | critical | 0 | 46 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 4281 | 3858 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 2519 | 512 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 2367 | 2105 | static | https://www.tarotnow.xyz/_next/static/chunks/0pd6iwgval8kp.js |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | GET | 200 | 2223 | 2077 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-out | desktop | auth-public | /vi/register | GET | 200 | 2092 | 1467 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | reading | /vi/reading/session/33d0cbf1-f592-4e7f-a9ab-3513d4ab39f2 | GET | 200 | 2026 | 673 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 2023 | 569 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 2004 | 1041 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 1832 | 1816 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 1806 | 1256 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-out | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1735 | 807 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1700 | 1005 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 1690 | 1633 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 1532 | 209 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 1432 | 1400 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | reading | /vi/reading/session/33d0cbf1-f592-4e7f-a9ab-3513d4ab39f2 | GET | 200 | 1391 | 1375 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 1382 | 1362 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 1345 | 331 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 1343 | 702 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | admin | /vi/admin/readings | GET | 200 | 1302 | 1287 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1263 | 599 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1207 | 1173 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 1170 | 334 | html | https://www.tarotnow.xyz/vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d |
| logged-out | desktop | auth-public | /vi/reset-password | GET | 200 | 1168 | 1157 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 1167 | 413 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 1159 | 232 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1154 | 1130 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 1131 | 318 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 1120 | 1103 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 1087 | 1069 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 1085 | 272 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 1072 | 1069 | static | https://www.tarotnow.xyz/_next/static/chunks/0vp4mbwt0gpbv.js |
| logged-out | mobile | auth-public | /vi/login | GET | 200 | 1065 | 1040 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 1057 | 217 | static | https://www.tarotnow.xyz/_next/static/chunks/13onjvrcl5bfv.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 1056 | 541 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1053 | 583 | html | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1052 | 562 | static | https://www.tarotnow.xyz/_next/static/chunks/04qliyftvi87..js |
| logged-in-admin | desktop | auth-public | /vi | GET | 200 | 1050 | 344 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 1035 | 675 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 1010 | 729 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1006 | 993 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 996 | 750 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 995 | 982 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-out | desktop | auth-public | /vi/legal/tos | GET | 200 | 993 | 977 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 973 | 950 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-out | desktop | auth-public | /vi/reset-password | GET | 200 | 967 | 956 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 955 | 947 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=96&q=75 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 951 | 321 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 951 | 145 | static | https://www.tarotnow.xyz/_next/static/chunks/04qliyftvi87..js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 949 | 929 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Ffree_draw_ticket_50_20260416_165452.avif&w=96&q=75 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 945 | 931 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 939 | 344 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 938 | 761 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 937 | 145 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-out | mobile | auth-public | /vi/reset-password | GET | 200 | 934 | 856 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 927 | 899 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 923 | 912 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=96&q=75 |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 922 | 138 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 922 | 909 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fexp_booster_50_20260416_165452.avif&w=96&q=75 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 919 | 903 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fdefense_booster_50_20260416_165452.avif&w=96&q=75 |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 908 | 134 | static | https://www.tarotnow.xyz/_next/static/chunks/04qliyftvi87..js |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 907 | 105 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-out | mobile | auth-public | /vi/register | GET | 200 | 895 | 882 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 894 | 273 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 893 | 879 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 884 | 319 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 879 | 657 | static | https://www.tarotnow.xyz/_next/static/chunks/04qliyftvi87..js |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 877 | 206 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 876 | 318 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 875 | 865 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fdefense_booster_50_20260416_165452.avif&w=48&q=75 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 869 | 321 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 865 | 292 | static | https://www.tarotnow.xyz/_next/static/chunks/0pd6iwgval8kp.js |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 865 | 288 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 864 | 206 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 862 | 717 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 860 | 685 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 859 | 693 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 857 | 851 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=48&q=75 |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 856 | 134 | static | https://www.tarotnow.xyz/_next/static/chunks/038oi6tq.i16~.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 855 | 844 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fexp_booster_50_20260416_165452.avif&w=48&q=75 |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 798 | 318 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 797 | 714 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 797 | 323 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 796 | 550 | static | https://www.tarotnow.xyz/_next/static/chunks/0xy7p7~l6scxr.js |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 795 | 399 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 794 | 783 | static | https://www.tarotnow.xyz/_next/static/chunks/13onjvrcl5bfv.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 792 | 321 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-out | desktop | auth-public | /vi | GET | 200 | 791 | 261 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | desktop | reading | /vi/reading/session/eacbcb32-5bb0-4271-bd9b-0c5345f67219 | GET | 200 | 790 | 329 | html | https://www.tarotnow.xyz/vi/reading/session/eacbcb32-5bb0-4271-bd9b-0c5345f67219 |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 786 | 658 | static | https://www.tarotnow.xyz/_next/static/chunks/0jt0d84fr1c7m.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 785 | 767 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 784 | 761 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 783 | 534 | static | https://www.tarotnow.xyz/_next/static/chunks/0jt0d84fr1c7m.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 783 | 769 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 783 | 324 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 782 | 325 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-out | desktop | auth-public | /vi | GET | 200 | 781 | 710 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 780 | 319 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 780 | 374 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 779 | 329 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 775 | 326 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 775 | 314 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 775 | 312 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 775 | 310 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | reading | /vi/reading/session/6f456328-dcf6-424c-825e-9385d76fc19d | GET | 200 | 775 | 335 | html | https://www.tarotnow.xyz/vi/reading/session/6f456328-dcf6-424c-825e-9385d76fc19d |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 773 | 322 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 773 | 315 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 772 | 759 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Ffree_draw_ticket_50_20260416_165452.avif&w=48&q=75 |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 772 | 305 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 772 | 314 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | admin | /vi/admin/deposits | GET | 200 | 771 | 331 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 771 | 310 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 771 | 315 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 771 | 313 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 770 | 319 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 770 | 306 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 770 | 381 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 770 | 321 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 769 | 317 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 766 | 341 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 765 | 314 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 765 | 299 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 764 | 206 | static | https://www.tarotnow.xyz/_next/static/chunks/0o0619o11z6de.js |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 764 | 675 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 762 | 331 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 761 | 230 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 759 | 750 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=48&q=75 |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 758 | 205 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 752 | 351 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 752 | 659 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | admin | /vi/admin | GET | 200 | 751 | 666 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 747 | 143 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 745 | 316 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | mobile | auth-public | /vi/register | GET | 200 | 744 | 325 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 743 | 329 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 743 | 308 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 738 | 719 | static | https://www.tarotnow.xyz/_next/static/chunks/13onjvrcl5bfv.js |
| logged-out | mobile | auth-public | /vi/reset-password | GET | 200 | 737 | 669 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 734 | 336 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 733 | 444 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-out | mobile | auth-public | /vi/register | GET | 200 | 732 | 599 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 730 | 366 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 730 | 292 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-out | mobile | auth-public | /vi/register | GET | 200 | 729 | 718 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 726 | 135 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 725 | 315 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 724 | 399 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 724 | 322 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 723 | 325 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | GET | 200 | 723 | 339 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | desktop | reading | /vi/reading/session/d3b73fcf-fe77-4fce-88b0-452d93bbfa7b | GET | 200 | 722 | 324 | html | https://www.tarotnow.xyz/vi/reading/session/d3b73fcf-fe77-4fce-88b0-452d93bbfa7b |
| logged-out | desktop | auth-public | /vi/legal/tos | GET | 200 | 721 | 342 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | desktop | admin | /vi/admin/readings | GET | 200 | 721 | 352 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-out | mobile | auth-public | /vi/login | GET | 200 | 719 | 682 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 719 | 346 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 718 | 328 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 718 | 331 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 716 | 135 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | desktop | reading | /vi/reading/session/5ea5a001-2465-4ba2-911f-1e0ddc5bc86d | GET | 200 | 715 | 136 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 715 | 281 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | desktop | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-reader | desktop | /vi/profile/mfa | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | desktop | /vi/wallet | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | desktop | /vi/wallet | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/media/2a65768255d6b625-s.14by5b4al-y~f.woff2 |
| logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/media/b49b0d9b851e4899-s.0yfy_qj1.2qn0.woff2 |
| logged-in-admin | mobile | /vi/wallet | https://www.tarotnow.xyz/_next/static/media/14e23f9b59180572-s.08.c8psu~gif9.woff2 |
| logged-in-reader | mobile | /vi/gamification | https://www.tarotnow.xyz/vi/gamification |

## Coverage Notes
| Scenario | Viewport | Note |
| --- | --- | --- |
| logged-out | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-out | desktop | dynamic-routes: skipped for logged-out scenario. |
| logged-out | desktop | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | desktop | reading.init.daily_1: blocked (400). |
| logged-in-admin | desktop | reading.init.spread_3: created d3b73fcf-fe77-4fce-88b0-452d93bbfa7b. |
| logged-in-admin | desktop | reading.init.spread_5: created 5ea5a001-2465-4ba2-911f-1e0ddc5bc86d. |
| logged-in-admin | desktop | reading.init.spread_10: created 2a0a3080-de77-4852-9a58-b3d1e92fff2f. |
| logged-in-admin | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | desktop | reader-detail:ui-discovery-empty |
| logged-in-admin | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-admin | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-admin | desktop | community-posts:api-discovery-1 |
| logged-in-admin | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | desktop | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | desktop | reading.init.daily_1: blocked (400). |
| logged-in-reader | desktop | reading.init.spread_3: created eacbcb32-5bb0-4271-bd9b-0c5345f67219. |
| logged-in-reader | desktop | reading.init.spread_5: created ba4ed496-3f1c-4bff-8ff1-4527fcb1cf44. |
| logged-in-reader | desktop | reading.init.spread_10: created f7987eb3-81ef-4f1b-8d95-19a85bdd6aae. |
| logged-in-reader | desktop | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | desktop | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | desktop | reader-detail:ui-discovery-empty |
| logged-in-reader | desktop | chat-room-detail:ui-discovery-empty |
| logged-in-reader | desktop | reading-history-detail:ui-discovery-empty |
| logged-in-reader | desktop | community-posts:api-discovery-1 |
| logged-in-reader | desktop | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | desktop | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |
| logged-out | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-out | mobile | dynamic-routes: skipped for logged-out scenario. |
| logged-out | mobile | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | mobile | reading.init.daily_1: blocked (400). |
| logged-in-admin | mobile | reading.init.spread_3: created 33d0cbf1-f592-4e7f-a9ab-3513d4ab39f2. |
| logged-in-admin | mobile | reading.init.spread_5: created 87c68e71-1ef8-4aa9-8a53-7ca6a3e1a353. |
| logged-in-admin | mobile | reading.init.spread_10: created 6d3e854c-16d8-49dc-8b91-086f1421b9bc. |
| logged-in-admin | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-admin | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-admin | mobile | reader-detail:ui-discovery-empty |
| logged-in-admin | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-admin | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-admin | mobile | community-posts:api-discovery-1 |
| logged-in-admin | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-admin | mobile | scenario-filter:admin-auth-entry-routes-skipped=5 |
| logged-in-reader | mobile | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-reader | mobile | origin-discovery:/robots.txt:routes-22 |
| logged-in-reader | mobile | reading.init.daily_1: blocked (400). |
| logged-in-reader | mobile | reading.init.spread_3: created ac436e4f-fad9-4cdc-bb04-edfbbb70f7dd. |
| logged-in-reader | mobile | reading.init.spread_5: created 6f456328-dcf6-424c-825e-9385d76fc19d. |
| logged-in-reader | mobile | reading.init.spread_10: created 306aa5d9-2a5c-4068-9fb0-74d97a4a410a. |
| logged-in-reader | mobile | reading-history-detail: coverage-blocked (no history id found). |
| logged-in-reader | mobile | chat-room-detail: coverage-blocked (no conversation id found). |
| logged-in-reader | mobile | reader-detail:ui-discovery-empty |
| logged-in-reader | mobile | chat-room-detail:ui-discovery-empty |
| logged-in-reader | mobile | reading-history-detail:ui-discovery-empty |
| logged-in-reader | mobile | community-posts:api-discovery-1 |
| logged-in-reader | mobile | community-post-detail:69db54fc297f66f734421a3c:stale-page-404 |
| logged-in-reader | mobile | scenario-filter:reader-auth-entry-admin-routes-skipped=15 |

## Login Bootstrap Notes
### logged-in-admin / desktop
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.
