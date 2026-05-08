# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T20:10:48.971Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 0
- High pages (request count): 143
- High slow requests: 274
- Medium slow requests: 699

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2877 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 3032 | 1243 | 0 | 0 | 14 | 0 | yes |
| logged-in-reader | desktop | 33 | 3124 | 954 | 0 | 0 | 18 | 0 | yes |
| logged-out | mobile | 9 | 2963 | 226 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 3416 | 1254 | 1 | 0 | 15 | 0 | yes |
| logged-in-reader | mobile | 33 | 3532 | 960 | 2 | 0 | 13 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2892 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2904 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6705 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4070 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.0 | 2939 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 3424 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2894 | 0 |
| logged-in-admin | desktop | inventory | 1 | 31.0 | 2917 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3207 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.7 | 2934 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2914 | 0 |
| logged-in-admin | desktop | profile | 3 | 28.7 | 2967 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2836 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.3 | 2827 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.4 | 2871 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.5 | 2961 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.5 | 3333 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 3384 | 0 |
| logged-in-admin | mobile | collection | 1 | 30.0 | 7437 | 0 |
| logged-in-admin | mobile | community | 1 | 35.0 | 5267 | 0 |
| logged-in-admin | mobile | gacha | 2 | 32.0 | 2933 | 0 |
| logged-in-admin | mobile | gamification | 1 | 31.0 | 3181 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2717 | 0 |
| logged-in-admin | mobile | inventory | 1 | 30.0 | 2897 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 3991 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2823 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2783 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.7 | 3633 | 1 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2834 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.1 | 3221 | 0 |
| logged-in-admin | mobile | reading | 5 | 31.8 | 3993 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.0 | 2829 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2763 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6860 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3575 | 0 |
| logged-in-reader | desktop | gacha | 2 | 30.5 | 2921 | 0 |
| logged-in-reader | desktop | gamification | 1 | 30.0 | 2916 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2751 | 0 |
| logged-in-reader | desktop | inventory | 1 | 32.0 | 2924 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 32.0 | 2917 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2792 | 0 |
| logged-in-reader | desktop | notifications | 1 | 30.0 | 2917 | 0 |
| logged-in-reader | desktop | profile | 3 | 30.0 | 3176 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2777 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.4 | 3169 | 0 |
| logged-in-reader | desktop | reading | 5 | 30.4 | 3068 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.0 | 2858 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 3824 | 0 |
| logged-in-reader | mobile | collection | 1 | 31.0 | 7411 | 0 |
| logged-in-reader | mobile | community | 1 | 34.0 | 4031 | 1 |
| logged-in-reader | mobile | gacha | 2 | 30.5 | 2937 | 0 |
| logged-in-reader | mobile | gamification | 1 | 31.0 | 2981 | 0 |
| logged-in-reader | mobile | home | 1 | 34.0 | 3048 | 0 |
| logged-in-reader | mobile | inventory | 1 | 30.0 | 2933 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 32.0 | 4212 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2894 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2926 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.3 | 4149 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2857 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.0 | 3963 | 0 |
| logged-in-reader | mobile | reading | 5 | 30.2 | 3230 | 1 |
| logged-in-reader | mobile | wallet | 4 | 28.3 | 2857 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2745 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3932 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2746 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2896 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3956 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2743 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3932 | 1319 | 1913 | 1064 | 1676 | 0.0000 | 165.0 | 600837 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2741 | 732 | 732 | 568 | 568 | 0.0000 | 0.0 | 511705 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2751 | 737 | 737 | 560 | 560 | 0.0000 | 0.0 | 512247 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2704 | 690 | 690 | 488 | 488 | 0.0000 | 0.0 | 511282 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2836 | 819 | 819 | 572 | 572 | 0.0000 | 0.0 | 511465 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2692 | 682 | 682 | 504 | 504 | 0.0000 | 0.0 | 511496 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2793 | 776 | 776 | 532 | 856 | 0.0000 | 0.0 | 525133 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 735 | 735 | 476 | 788 | 0.0000 | 0.0 | 525449 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2700 | 684 | 684 | 540 | 876 | 0.0000 | 0.0 | 525377 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2894 | 882 | 882 | 764 | 1332 | 0.0041 | 265.0 | 536944 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2950 | 898 | 934 | 596 | 1012 | 0.0041 | 0.0 | 642144 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 31 | 8 | high | 0 | 0 | 1 | 0 | 5 | 2 | 3 | 5 | 0 | 0 | 2917 | 903 | 904 | 536 | 1176 | 0.0041 | 0.0 | 644676 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 31 | 10 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2938 | 896 | 930 | 552 | 1236 | 0.0041 | 0.0 | 725252 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2939 | 890 | 890 | 552 | 1252 | 0.0041 | 0.0 | 726573 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 27 | high | 0 | 0 | 0 | 0 | 14 | 4 | 5 | 13 | 1 | 0 | 6705 | 798 | 811 | 548 | 932 | 0.0042 | 0.0 | 642814 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3205 | 982 | 1190 | 560 | 1056 | 0.0489 | 0.0 | 634302 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2936 | 838 | 924 | 580 | 992 | 0.0041 | 0.0 | 631209 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2761 | 912 | 1104 | 592 | 996 | 0.0489 | 0.0 | 630171 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2940 | 885 | 886 | 588 | 984 | 0.0041 | 0.0 | 633555 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2904 | 890 | 894 | 684 | 1124 | 0.0041 | 0.0 | 631156 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3207 | 876 | 1196 | 540 | 1164 | 0.0041 | 0.0 | 651737 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | 4 | high | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 4070 | 795 | 1288 | 588 | 1928 | 0.0041 | 0.0 | 777084 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3424 | 937 | 1392 | 572 | 1008 | 0.0279 | 0.0 | 643190 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3017 | 954 | 977 | 556 | 1200 | 0.0041 | 0.0 | 633709 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2937 | 916 | 918 | 608 | 1024 | 0.0041 | 0.0 | 631715 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2996 | 972 | 973 | 536 | 1280 | 0.0041 | 0.0 | 632595 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 37 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2893 | 906 | 906 | 536 | 1268 | 0.0041 | 0.0 | 630660 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2914 | 852 | 876 | 548 | 1000 | 0.0041 | 0.0 | 631663 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2836 | 819 | 824 | 548 | 968 | 0.0041 | 0.0 | 632031 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2892 | 832 | 876 | 544 | 1116 | 0.0041 | 0.0 | 632398 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2886 | 867 | 867 | 588 | 904 | 0.0020 | 0.0 | 525580 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2871 | 856 | 859 | 592 | 932 | 0.0020 | 0.0 | 525501 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 27 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3045 | 1023 | 1024 | 696 | 1068 | 0.0020 | 0.0 | 527490 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3208 | 851 | 1195 | 548 | 924 | 0.0000 | 0.0 | 647108 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 742 | 743 | 500 | 940 | 0.0000 | 0.0 | 647073 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2766 | 749 | 750 | 540 | 860 | 0.0000 | 0.0 | 645380 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3435 | 972 | 1389 | 752 | 1088 | 0.0022 | 0.0 | 698841 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2776 | 756 | 760 | 568 | 900 | 0.0000 | 0.0 | 644084 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2766 | 745 | 749 | 520 | 892 | 0.0000 | 0.0 | 646018 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2871 | 834 | 839 | 568 | 1088 | 0.0000 | 0.0 | 648087 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2739 | 720 | 722 | 572 | 1064 | 0.0000 | 0.0 | 655383 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2841 | 815 | 818 | 512 | 1176 | 0.0000 | 0.0 | 649442 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2751 | 731 | 738 | 540 | 888 | 0.0000 | 0.0 | 645557 |
| logged-in-admin | desktop | reading | /vi/reading/session/c9e74e2c-ca13-445e-bb7c-88531f4a74f9 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3008 | 783 | 994 | 560 | 948 | 0.0041 | 0.0 | 724729 |
| logged-in-admin | desktop | reading | /vi/reading/session/e1ba9861-ab88-4453-a41e-c6105453c3f4 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2748 | 735 | 735 | 540 | 940 | 0.0041 | 0.0 | 631938 |
| logged-in-admin | desktop | reading | /vi/reading/session/0d5883f6-5e8c-4baa-9786-8bcff5e110ce | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 736 | 741 | 504 | 944 | 0.0041 | 0.0 | 631719 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2750 | 732 | 733 | 564 | 940 | 0.0041 | 0.0 | 630565 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2720 | 702 | 709 | 552 | 932 | 0.0041 | 0.0 | 630754 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2969 | 813 | 819 | 584 | 1000 | 0.0041 | 0.0 | 631874 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2788 | 763 | 768 | 536 | 1080 | 0.0041 | 0.0 | 632467 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2837 | 812 | 818 | 580 | 1140 | 0.0041 | 0.0 | 633356 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2784 | 761 | 762 | 588 | 1208 | 0.0041 | 0.0 | 632398 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2751 | 718 | 737 | 680 | 1268 | 0.0038 | 313.0 | 537027 |
| logged-in-reader | desktop | reading | /vi/reading | 30 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3002 | 956 | 982 | 560 | 1032 | 0.0039 | 0.0 | 642989 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 2 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2924 | 888 | 889 | 548 | 1196 | 0.0039 | 0.0 | 645884 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 31 | 10 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2916 | 871 | 871 | 540 | 1264 | 0.0039 | 0.0 | 725629 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2926 | 876 | 877 | 572 | 1312 | 0.0039 | 0.0 | 723733 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 25 | high | 0 | 0 | 0 | 0 | 6 | 4 | 0 | 6 | 0 | 0 | 6860 | 757 | 779 | 552 | 552 | 0.0040 | 0.0 | 641828 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3412 | 1087 | 1314 | 580 | 1224 | 0.0726 | 0.0 | 638378 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3364 | 1193 | 1234 | 940 | 940 | 0.0039 | 0.0 | 631636 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2751 | 727 | 730 | 560 | 1344 | 0.0039 | 0.0 | 631978 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2926 | 870 | 870 | 580 | 1004 | 0.0039 | 0.0 | 634415 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2763 | 724 | 747 | 552 | 1008 | 0.0039 | 0.0 | 631327 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2917 | 892 | 892 | 548 | 1208 | 0.0039 | 0.0 | 652300 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | 9 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3575 | 815 | 830 | 660 | 1844 | 0.0039 | 0.0 | 642607 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2916 | 899 | 904 | 672 | 1132 | 0.0277 | 0.0 | 643239 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 845 | 845 | 572 | 1172 | 0.0039 | 0.0 | 633769 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2770 | 738 | 754 | 540 | 952 | 0.0039 | 0.0 | 631106 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2888 | 818 | 826 | 552 | 920 | 0.0039 | 0.0 | 631177 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2906 | 851 | 851 | 556 | 1188 | 0.0095 | 0.0 | 632358 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 30 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2917 | 898 | 898 | 504 | 940 | 0.0040 | 0.0 | 633509 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2777 | 737 | 760 | 500 | 920 | 0.0039 | 0.0 | 632178 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2793 | 775 | 777 | 576 | 1176 | 0.0039 | 0.0 | 632209 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2703 | 684 | 684 | 524 | 880 | 0.0019 | 0.0 | 525609 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2826 | 795 | 795 | 592 | 920 | 0.0019 | 0.0 | 525624 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2848 | 822 | 823 | 544 | 888 | 0.0019 | 0.0 | 525796 |
| logged-in-reader | desktop | reading | /vi/reading/session/e3d1b803-033a-4297-b74a-11f8aa7ea237 | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2926 | 879 | 908 | 676 | 1068 | 0.0039 | 0.0 | 631711 |
| logged-in-reader | desktop | reading | /vi/reading/session/c60ad343-16a2-4bb9-840f-9f51668471f6 | 35 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3150 | 928 | 1129 | 624 | 1072 | 0.0039 | 0.0 | 725735 |
| logged-in-reader | desktop | reading | /vi/reading/session/67123054-0814-48a4-9a76-313512697271 | 31 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3470 | 781 | 1447 | 532 | 908 | 0.0039 | 0.0 | 712302 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2747 | 707 | 728 | 556 | 960 | 0.0039 | 0.0 | 630767 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3105 | 1034 | 1047 | 720 | 1068 | 0.0039 | 0.0 | 631278 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2964 | 920 | 940 | 564 | 992 | 0.0039 | 0.0 | 631479 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2893 | 765 | 783 | 556 | 1184 | 0.0039 | 0.0 | 632201 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3780 | 1068 | 1085 | 1132 | 1132 | 0.0039 | 0.0 | 632446 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3771 | 1136 | 1152 | 812 | 812 | 0.0039 | 0.0 | 632534 |
| logged-out | mobile | auth-public | /vi | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3956 | 1448 | 1694 | 1508 | 1508 | 0.0000 | 38.0 | 602052 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3138 | 692 | 872 | 600 | 600 | 0.0000 | 0.0 | 511665 |
| logged-out | mobile | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3016 | 935 | 936 | 540 | 540 | 0.0000 | 0.0 | 512833 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2705 | 690 | 690 | 500 | 500 | 0.0000 | 0.0 | 511309 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2856 | 804 | 842 | 624 | 624 | 0.0000 | 0.0 | 511403 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2766 | 745 | 745 | 504 | 504 | 0.0000 | 0.0 | 511528 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2751 | 736 | 736 | 540 | 880 | 0.0000 | 0.0 | 525222 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 749 | 750 | 456 | 784 | 0.0000 | 0.0 | 525184 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2710 | 691 | 691 | 460 | 792 | 0.0000 | 0.0 | 525310 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2717 | 668 | 668 | 464 | 848 | 0.0032 | 0.0 | 537024 |
| logged-in-admin | mobile | reading | /vi/reading | 30 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2905 | 846 | 877 | 464 | 840 | 0.0000 | 0.0 | 642875 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 30 | 9 | high | 0 | 0 | 0 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 2897 | 875 | 876 | 488 | 1140 | 0.0000 | 0.0 | 643993 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 2 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2899 | 867 | 867 | 484 | 1144 | 0.0000 | 0.0 | 726205 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2966 | 926 | 929 | 496 | 868 | 0.0000 | 0.0 | 727394 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 30 | 26 | high | 0 | 0 | 1 | 0 | 13 | 4 | 4 | 12 | 1 | 0 | 7437 | 806 | 868 | 536 | 876 | 0.0000 | 305.0 | 643490 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3279 | 881 | 1100 | 460 | 1140 | 0.0689 | 0.0 | 634398 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4049 | 1132 | 1147 | 968 | 968 | 0.0000 | 0.0 | 630852 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 36 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3570 | 1483 | 1603 | 1192 | 1192 | 0.0878 | 0.0 | 629969 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4089 | 1262 | 1268 | 572 | 936 | 0.0071 | 0.0 | 633277 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3384 | 1068 | 1076 | 568 | 936 | 0.0071 | 0.0 | 631251 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3991 | 1077 | 1077 | 660 | 976 | 0.0000 | 0.0 | 650541 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 35 | 4 | high | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 5267 | 914 | 1510 | 896 | 2436 | 0.0051 | 1.0 | 776537 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3181 | 1120 | 1121 | 652 | 1056 | 0.0000 | 0.0 | 644537 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2900 | 869 | 878 | 512 | 864 | 0.0000 | 0.0 | 633713 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 727 | 736 | 548 | 892 | 0.0000 | 0.0 | 631109 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2809 | 737 | 782 | 452 | 804 | 0.0000 | 0.0 | 631919 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 36 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2852 | 848 | 849 | 484 | 1140 | 0.0071 | 0.0 | 630690 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2783 | 723 | 763 | 492 | 836 | 0.0000 | 0.0 | 631525 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2834 | 750 | 818 | 468 | 816 | 0.0000 | 0.0 | 632095 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2893 | 794 | 869 | 508 | 880 | 0.0000 | 0.0 | 632300 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2844 | 804 | 804 | 480 | 800 | 0.0032 | 0.0 | 525573 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2773 | 728 | 728 | 480 | 804 | 0.0032 | 0.0 | 525673 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2852 | 798 | 798 | 524 | 860 | 0.0032 | 0.0 | 525695 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2843 | 788 | 815 | 456 | 812 | 0.0000 | 0.0 | 647088 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2974 | 778 | 848 | 568 | 928 | 0.0000 | 0.0 | 647061 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2839 | 781 | 806 | 484 | 824 | 0.0000 | 0.0 | 645331 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2998 | 795 | 930 | 480 | 808 | 0.0000 | 0.0 | 697978 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2899 | 791 | 827 | 508 | 852 | 0.0000 | 0.0 | 644109 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3002 | 766 | 798 | 488 | 828 | 0.0000 | 0.0 | 645906 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3992 | 1170 | 1270 | 1084 | 1084 | 0.0000 | 0.0 | 648268 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3898 | 948 | 1541 | 540 | 888 | 0.0000 | 0.0 | 688703 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | 2 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3998 | 1255 | 1262 | 920 | 1316 | 0.0000 | 0.0 | 650224 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3884 | 1169 | 1223 | 984 | 1320 | 0.0000 | 0.0 | 645569 |
| logged-in-admin | mobile | reading | /vi/reading/session/50450bde-86d4-40b7-8aaf-bdcac8d36de3 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4179 | 937 | 1682 | 852 | 3384 | 0.0000 | 0.0 | 692731 |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4797 | 1697 | 2378 | 1108 | 4008 | 0.0001 | 0.0 | 692855 |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | 35 | 2 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5190 | 1480 | 2843 | 624 | 3796 | 0.0001 | 0.0 | 702356 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3863 | 1319 | 1346 | 532 | 852 | 0.0071 | 0.0 | 630734 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3107 | 840 | 852 | 500 | 856 | 0.0000 | 0.0 | 630894 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2790 | 758 | 769 | 508 | 824 | 0.0000 | 0.0 | 630600 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2894 | 787 | 851 | 508 | 856 | 0.0000 | 0.0 | 632223 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2859 | 815 | 837 | 484 | 852 | 0.0000 | 0.0 | 632545 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2944 | 889 | 906 | 488 | 848 | 0.0000 | 0.0 | 633229 |
| logged-in-reader | mobile | auth-public | /vi | 34 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3048 | 802 | 1005 | 808 | 808 | 0.0032 | 0.0 | 611166 |
| logged-in-reader | mobile | reading | /vi/reading | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2963 | 869 | 898 | 500 | 860 | 0.0000 | 0.0 | 641855 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 30 | 9 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2933 | 881 | 882 | 504 | 1164 | 0.0000 | 0.0 | 643955 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 31 | 9 | high | 0 | 0 | 1 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2989 | 945 | 946 | 484 | 1156 | 0.0000 | 0.0 | 725141 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2885 | 837 | 846 | 484 | 840 | 0.0000 | 0.0 | 723749 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 31 | 23 | high | 0 | 0 | 2 | 0 | 6 | 2 | 0 | 6 | 0 | 0 | 7411 | 924 | 956 | 464 | 832 | 0.0000 | 17.0 | 643335 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4012 | 1215 | 1215 | 856 | 1156 | 0.0000 | 0.0 | 637607 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3873 | 1251 | 1338 | 1292 | 1292 | 0.0000 | 0.0 | 631393 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4562 | 1299 | 1300 | 1056 | 1400 | 0.0000 | 0.0 | 631835 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3645 | 1037 | 1199 | 988 | 1340 | 0.0000 | 0.0 | 633633 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3824 | 955 | 1028 | 996 | 996 | 0.0000 | 0.0 | 631421 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4212 | 1654 | 1654 | 984 | 1340 | 0.0000 | 0.0 | 652381 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 34 | 5 | high | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 4031 | 863 | 889 | 684 | 1672 | 0.0051 | 0.0 | 776321 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 31 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2981 | 888 | 889 | 456 | 852 | 0.0000 | 0.0 | 643719 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2877 | 851 | 855 | 560 | 916 | 0.0000 | 0.0 | 633618 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2830 | 784 | 799 | 496 | 892 | 0.0000 | 0.0 | 631276 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2806 | 736 | 751 | 496 | 856 | 0.0000 | 0.0 | 631581 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2914 | 876 | 877 | 476 | 1120 | 0.0330 | 0.0 | 633323 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2926 | 807 | 835 | 468 | 864 | 0.0000 | 0.0 | 631979 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2857 | 806 | 810 | 516 | 892 | 0.0000 | 0.0 | 632173 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2855 | 759 | 809 | 464 | 840 | 0.0000 | 0.0 | 632442 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2773 | 717 | 738 | 476 | 840 | 0.0032 | 0.0 | 525604 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3095 | 891 | 920 | 712 | 1028 | 0.0032 | 0.0 | 525805 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2814 | 673 | 674 | 512 | 840 | 0.0032 | 0.0 | 525733 |
| logged-in-reader | mobile | reading | /vi/reading/session/c58d5277-b854-410b-a6c5-8a712ad1be5d | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3001 | 816 | 981 | 472 | 860 | 0.0001 | 0.0 | 693021 |
| logged-in-reader | mobile | reading | /vi/reading/session/a33d66ea-3e2e-4352-bbbc-91ba163ec266 | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2736 | 694 | 707 | 476 | 808 | 0.0000 | 0.0 | 632011 |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | 33 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4596 | 1338 | 2335 | 520 | 3656 | 0.0072 | 0.0 | 692823 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4328 | 1163 | 1175 | 1044 | 1368 | 0.0000 | 0.0 | 630878 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3590 | 737 | 889 | 840 | 840 | 0.0000 | 0.0 | 631025 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4315 | 1492 | 1522 | 1328 | 1328 | 0.0000 | 0.0 | 630917 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3724 | 810 | 910 | 760 | 1132 | 0.0000 | 0.0 | 632365 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3878 | 1061 | 1072 | 760 | 760 | 0.0000 | 0.0 | 632628 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4258 | 1272 | 1341 | 1000 | 1000 | 0.0000 | 0.0 | 632602 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 14 | 4 | 5 | 13 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 6 | 4 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 13 | 4 | 4 | 12 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 6 | 2 | 0 | 6 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 27 | high | 1 | 23 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/c9e74e2c-ca13-445e-bb7c-88531f4a74f9 | 34 | high | 2 | 30 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/e1ba9861-ab88-4453-a41e-c6105453c3f4 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/0d5883f6-5e8c-4baa-9786-8bcff5e110ce | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 31 | high | 1 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/e3d1b803-033a-4297-b74a-11f8aa7ea237 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/c60ad343-16a2-4bb9-840f-9f51668471f6 | 35 | high | 3 | 30 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/67123054-0814-48a4-9a76-313512697271 | 31 | high | 0 | 29 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 31 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/50450bde-86d4-40b7-8aaf-bdcac8d36de3 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | 35 | high | 3 | 30 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 34 | high | 4 | 27 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 31 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 34 | high | 0 | 32 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 31 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/c58d5277-b854-410b-a6c5-8a712ad1be5d | 33 | high | 2 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/a33d66ea-3e2e-4352-bbbc-91ba163ec266 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | 33 | high | 2 | 29 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1568 | 310 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1394 | 64 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1372 | 319 | html | https://www.tarotnow.xyz/vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1368 | 317 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1365 | 104 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1365 | 74 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1332 | 102 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1312 | 113 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1307 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1306 | 108 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1283 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1272 | 105 | static | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1267 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1253 | 72 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1240 | 83 | static | https://www.tarotnow.xyz/_next/static/chunks/03suy5k_kh24c.js |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1239 | 327 | html | https://www.tarotnow.xyz/vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1238 | 99 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1236 | 83 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1235 | 107 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1228 | 84 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1227 | 82 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1225 | 86 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1220 | 78 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1217 | 60 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1210 | 81 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1208 | 70 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1202 | 570 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1202 | 93 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1201 | 94 | static | https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1196 | 76 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1187 | 64 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1184 | 304 | html | https://www.tarotnow.xyz/vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1182 | 107 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1181 | 188 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1181 | 63 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1177 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/0h38g3weqde1h.js |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1176 | 121 | static | https://www.tarotnow.xyz/_next/static/chunks/0dg8~d_lbp93p.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1172 | 82 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1168 | 82 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1167 | 178 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1163 | 110 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1157 | 74 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1148 | 87 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1147 | 83 | static | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1144 | 82 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1144 | 142 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1136 | 312 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1136 | 100 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 1130 | 88 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1127 | 111 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1124 | 128 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 1122 | 357 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1121 | 312 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1121 | 305 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1119 | 290 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1118 | 297 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1117 | 124 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1114 | 286 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1114 | 133 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 1107 | 321 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1106 | 110 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1106 | 70 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1105 | 314 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1100 | 70 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1095 | 318 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 1092 | 93 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1091 | 544 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1090 | 105 | static | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1090 | 133 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1088 | 90 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1088 | 74 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1087 | 353 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1086 | 87 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1086 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 1085 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1084 | 81 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | reading | /vi/reading/session/46d9d8f4-7374-4a53-9748-3b3c987e0552 | GET | 200 | 1083 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1083 | 85 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 1078 | 77 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1070 | 76 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 800 | 75 | static | https://www.tarotnow.xyz/_next/static/chunks/0b5a588g_0r8q.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 799 | 327 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 798 | 327 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | GET | 200 | 798 | 336 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 798 | 319 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 797 | 85 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 796 | 318 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 796 | 323 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 796 | 309 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 795 | 73 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 793 | 59 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 793 | 72 | static | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 792 | 320 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-out | mobile | auth-public | /vi | GET | 200 | 792 | 728 | api | https://www.tarotnow.xyz/api/readers?page=1&pageSize=4 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 791 | 299 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 791 | 136 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 790 | 158 | static | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 788 | 77 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 788 | 302 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 787 | 316 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 787 | 139 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 786 | 327 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 786 | 322 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 786 | 126 | static | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | reader-chat | /vi/chat | GET | 200 | 786 | 99 | static | https://www.tarotnow.xyz/_next/static/chunks/0ryoba0b~me9m.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 786 | 314 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | GET | 200 | 786 | 536 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 786 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 785 | 380 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | GET | 200 | 785 | 344 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 784 | 326 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 784 | 107 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 781 | 71 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-reader | mobile | reader-chat | /vi/chat | GET | 200 | 779 | 96 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 778 | 307 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 777 | 549 | static | https://www.tarotnow.xyz/_next/static/chunks/0ybd~u7.9awcr.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 775 | 320 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 774 | 193 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-reader | mobile | reading | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | GET | 200 | 774 | 67 | static | https://www.tarotnow.xyz/themes/prismatic-royal.css |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 773 | 317 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 773 | 121 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 773 | 94 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 772 | 326 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | mobile | reader-chat | /vi/chat | GET | 200 | 772 | 90 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 770 | 391 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 770 | 116 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reader-chat | /vi/chat | GET | 200 | 770 | 107 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 769 | 110 | static | https://www.tarotnow.xyz/_next/static/chunks/turbopack-164lj7fr~s_z4.js |
| logged-in-admin | mobile | reading | /vi/reading/session/64fbd38d-62d7-4ac0-9e14-5135960096e5 | GET | 200 | 769 | 51 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 768 | 83 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 764 | 322 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 763 | 72 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-out | desktop | auth-public | /vi/reset-password | GET | 200 | 761 | 384 | html | https://www.tarotnow.xyz/vi/reset-password |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 761 | 244 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | mobile | reading | /vi/reading/session/50450bde-86d4-40b7-8aaf-bdcac8d36de3 | GET | 200 | 760 | 317 | html | https://www.tarotnow.xyz/vi/reading/session/50450bde-86d4-40b7-8aaf-bdcac8d36de3 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 760 | 92 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 759 | 45 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 759 | 59 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 758 | 106 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | reader-chat | /vi/chat | GET | 200 | 757 | 117 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqce2yjfryre.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 757 | 65 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 756 | 59 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 755 | 80 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 754 | 419 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 753 | 331 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 753 | 70 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 752 | 87 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 748 | 77 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 747 | 100 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 747 | 340 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fui-avatars.com%2Fapi%2F%3Fbackground%3D111%26color%3Dfff%26name%3DTest&w=384&q=75 |
| logged-in-admin | desktop | reading | /vi/reading/history | GET | 200 | 746 | 324 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 746 | 346 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 745 | 333 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 744 | 318 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | mobile | reading | /vi/reading/session/50450bde-86d4-40b7-8aaf-bdcac8d36de3 | GET | 200 | 743 | 103 | static | https://www.tarotnow.xyz/_next/static/chunks/0kn_-kiqq.6et.js |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 742 | 108 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 742 | 65 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 740 | 72 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | desktop | admin | /vi/admin/users | GET | 200 | 738 | 318 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 738 | 322 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/community | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-reader | mobile | /vi/reading/session/1d407a36-aa47-4119-8b9a-c8fbdd48b8f9 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |

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
| logged-in-admin | desktop | reading.init.spread_3: created c9e74e2c-ca13-445e-bb7c-88531f4a74f9. |
| logged-in-admin | desktop | reading.init.spread_5: created e1ba9861-ab88-4453-a41e-c6105453c3f4. |
| logged-in-admin | desktop | reading.init.spread_10: created 0d5883f6-5e8c-4baa-9786-8bcff5e110ce. |
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
| logged-in-reader | desktop | reading.init.spread_3: created e3d1b803-033a-4297-b74a-11f8aa7ea237. |
| logged-in-reader | desktop | reading.init.spread_5: created c60ad343-16a2-4bb9-840f-9f51668471f6. |
| logged-in-reader | desktop | reading.init.spread_10: created 67123054-0814-48a4-9a76-313512697271. |
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
| logged-in-admin | mobile | reading.init.spread_3: created 50450bde-86d4-40b7-8aaf-bdcac8d36de3. |
| logged-in-admin | mobile | reading.init.spread_5: created 46d9d8f4-7374-4a53-9748-3b3c987e0552. |
| logged-in-admin | mobile | reading.init.spread_10: created 64fbd38d-62d7-4ac0-9e14-5135960096e5. |
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
| logged-in-reader | mobile | reading.init.spread_3: created c58d5277-b854-410b-a6c5-8a712ad1be5d. |
| logged-in-reader | mobile | reading.init.spread_5: created a33d66ea-3e2e-4352-bbbc-91ba163ec266. |
| logged-in-reader | mobile | reading.init.spread_10: created 1d407a36-aa47-4119-8b9a-c8fbdd48b8f9. |
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
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / desktop
- Attempt 1: login bootstrap succeeded.

### logged-in-admin / mobile
- Attempt 1: login bootstrap succeeded.

### logged-in-reader / mobile
- Attempt 1: login bootstrap succeeded.
