# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-09T00:26:06.346Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 2
- High pages (request count): 144
- High slow requests: 45
- Medium slow requests: 282

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2886 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 44 | 2953 | 1273 | 0 | 0 | 14 | 0 | yes |
| logged-in-reader | desktop | 34 | 2926 | 970 | 0 | 0 | 7 | 0 | yes |
| logged-out | mobile | 9 | 2857 | 226 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 3058 | 1278 | 0 | 0 | 13 | 0 | yes |
| logged-in-reader | mobile | 33 | 2960 | 961 | 0 | 0 | 18 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.6 | 2927 | 0 |
| logged-in-admin | desktop | chat | 1 | 30.0 | 3305 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6337 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 3999 | 0 |
| logged-in-admin | desktop | gacha | 2 | 31.0 | 2861 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 3291 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2695 | 0 |
| logged-in-admin | desktop | inventory | 1 | 31.0 | 2869 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3204 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2655 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2726 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2888 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2728 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.0 | 2759 | 0 |
| logged-in-admin | desktop | reading | 6 | 29.2 | 2772 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.5 | 2808 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.4 | 2848 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 3898 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5771 | 0 |
| logged-in-admin | mobile | community | 1 | 36.0 | 3935 | 0 |
| logged-in-admin | mobile | gacha | 2 | 30.0 | 2912 | 0 |
| logged-in-admin | mobile | gamification | 1 | 32.0 | 3474 | 0 |
| logged-in-admin | mobile | home | 1 | 34.0 | 2941 | 0 |
| logged-in-admin | mobile | inventory | 1 | 30.0 | 2925 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 3246 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2762 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2838 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.7 | 2877 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2759 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2886 | 0 |
| logged-in-admin | mobile | reading | 5 | 29.2 | 2836 | 0 |
| logged-in-admin | mobile | wallet | 4 | 36.0 | 3531 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2758 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6230 | 0 |
| logged-in-reader | desktop | community | 1 | 35.0 | 3620 | 0 |
| logged-in-reader | desktop | gacha | 2 | 30.5 | 2867 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 2847 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2765 | 0 |
| logged-in-reader | desktop | inventory | 1 | 30.0 | 2836 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2802 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2711 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2843 | 0 |
| logged-in-reader | desktop | profile | 3 | 28.7 | 2890 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2723 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.0 | 2792 | 0 |
| logged-in-reader | desktop | reading | 6 | 29.2 | 2801 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.5 | 2793 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2754 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5585 | 0 |
| logged-in-reader | mobile | community | 1 | 35.0 | 3657 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.5 | 2917 | 0 |
| logged-in-reader | mobile | gamification | 1 | 30.0 | 2881 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2878 | 0 |
| logged-in-reader | mobile | inventory | 1 | 31.0 | 2891 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2849 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2735 | 0 |
| logged-in-reader | mobile | notifications | 1 | 30.0 | 2888 | 0 |
| logged-in-reader | mobile | profile | 3 | 28.7 | 2890 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2816 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.4 | 2838 | 0 |
| logged-in-reader | mobile | reading | 5 | 30.4 | 2875 | 0 |
| logged-in-reader | mobile | wallet | 4 | 28.8 | 2880 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2755 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4065 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2710 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2734 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3529 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2837 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4065 | 1517 | 1915 | 1516 | 1516 | 0.0000 | 522.0 | 600939 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2961 | 952 | 952 | 744 | 744 | 0.0000 | 0.0 | 511635 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2703 | 693 | 693 | 512 | 512 | 0.0000 | 0.0 | 512233 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2672 | 664 | 664 | 524 | 524 | 0.0000 | 0.0 | 511270 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2713 | 706 | 706 | 480 | 480 | 0.0000 | 0.0 | 511522 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2727 | 721 | 721 | 496 | 496 | 0.0000 | 0.0 | 511488 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2635 | 629 | 629 | 436 | 768 | 0.0000 | 0.0 | 525325 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2682 | 671 | 671 | 472 | 472 | 0.0000 | 0.0 | 525322 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2812 | 803 | 803 | 480 | 804 | 0.0000 | 0.0 | 525494 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2695 | 684 | 685 | 624 | 1192 | 0.0035 | 228.0 | 537021 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2893 | 857 | 886 | 520 | 976 | 0.0041 | 0.0 | 642295 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 31 | 8 | high | 0 | 0 | 1 | 0 | 5 | 0 | 5 | 5 | 0 | 0 | 2869 | 864 | 864 | 520 | 1212 | 0.0041 | 0.0 | 644840 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 31 | 10 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2828 | 820 | 820 | 496 | 1184 | 0.0041 | 0.0 | 725500 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2893 | 872 | 872 | 488 | 1200 | 0.0041 | 0.0 | 726583 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 27 | high | 0 | 0 | 0 | 0 | 13 | 4 | 3 | 12 | 1 | 0 | 6337 | 752 | 764 | 540 | 540 | 0.0042 | 0.0 | 642810 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3100 | 827 | 1094 | 560 | 972 | 0.0538 | 0.0 | 635630 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2794 | 775 | 788 | 572 | 948 | 0.0041 | 0.0 | 630926 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2771 | 831 | 1465 | 516 | 1172 | 0.0489 | 0.0 | 630222 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2895 | 875 | 887 | 580 | 960 | 0.0041 | 0.0 | 633381 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 30 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3305 | 1292 | 1295 | 528 | 1384 | 0.0041 | 0.0 | 632742 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3204 | 694 | 1198 | 540 | 956 | 0.0041 | 0.0 | 650893 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | 4 | high | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3999 | 712 | 1228 | 512 | 2112 | 0.0041 | 0.0 | 777103 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3291 | 822 | 1283 | 528 | 936 | 0.0279 | 0.0 | 642900 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 30 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2876 | 865 | 865 | 512 | 1220 | 0.0041 | 0.0 | 635351 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 733 | 747 | 512 | 924 | 0.0041 | 0.0 | 630978 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2895 | 819 | 862 | 544 | 1132 | 0.0041 | 0.0 | 631851 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 37 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2706 | 845 | 845 | 504 | 1208 | 0.0041 | 0.0 | 630677 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2726 | 717 | 718 | 508 | 892 | 0.0046 | 0.0 | 631340 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2728 | 713 | 720 | 508 | 924 | 0.0041 | 0.0 | 632081 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2726 | 712 | 720 | 492 | 1084 | 0.0041 | 0.0 | 632180 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2686 | 676 | 676 | 504 | 872 | 0.0020 | 0.0 | 525604 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2661 | 653 | 653 | 480 | 832 | 0.0020 | 0.0 | 525723 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2618 | 608 | 608 | 488 | 816 | 0.0020 | 0.0 | 525706 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3195 | 688 | 1184 | 532 | 916 | 0.0000 | 0.0 | 647051 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2727 | 702 | 715 | 524 | 964 | 0.0000 | 0.0 | 647130 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 31 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2931 | 848 | 923 | 552 | 944 | 0.0000 | 0.0 | 648518 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3261 | 753 | 1255 | 548 | 840 | 0.0022 | 0.0 | 697782 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2826 | 800 | 809 | 512 | 900 | 0.0000 | 0.0 | 644114 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2832 | 799 | 814 | 496 | 880 | 0.0000 | 0.0 | 645778 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2720 | 709 | 710 | 548 | 952 | 0.0000 | 0.0 | 648191 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3023 | 930 | 1010 | 616 | 1172 | 0.0000 | 0.0 | 688896 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2896 | 883 | 885 | 580 | 1276 | 0.0000 | 0.0 | 649451 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2857 | 838 | 851 | 536 | 868 | 0.0000 | 0.0 | 645647 |
| logged-in-admin | desktop | reading | /vi/reading/session/dc12ad57-48d6-4e3f-bfa2-8d84f255a2d2 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2919 | 777 | 910 | 476 | 868 | 0.0044 | 0.0 | 724578 |
| logged-in-admin | desktop | reading | /vi/reading/session/174febce-76ad-4bac-bfd9-7471984a6b35 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2716 | 699 | 706 | 516 | 908 | 0.0041 | 0.0 | 631685 |
| logged-in-admin | desktop | reading | /vi/reading/session/44f456bd-915f-4f30-ba10-7b3bac6cf8f5 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2704 | 685 | 695 | 532 | 944 | 0.0041 | 0.0 | 631719 |
| logged-in-admin | desktop | reading | /vi/reading/session/8bf24139-8c59-4fa5-bbdb-477161895ddb | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2674 | 663 | 663 | 476 | 868 | 0.0041 | 0.0 | 631550 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2724 | 711 | 717 | 468 | 872 | 0.0041 | 0.0 | 630732 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2729 | 709 | 720 | 504 | 892 | 0.0041 | 0.0 | 630692 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2789 | 771 | 779 | 456 | 844 | 0.0041 | 0.0 | 630548 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2786 | 761 | 771 | 544 | 1080 | 0.0041 | 0.0 | 632425 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2668 | 657 | 658 | 480 | 1024 | 0.0041 | 0.0 | 632566 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2722 | 700 | 712 | 488 | 1060 | 0.0041 | 0.0 | 632530 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2765 | 722 | 753 | 544 | 1128 | 0.0033 | 262.0 | 537108 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2869 | 833 | 860 | 504 | 912 | 0.0039 | 0.0 | 642549 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 30 | 9 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2836 | 827 | 827 | 492 | 1168 | 0.0039 | 0.0 | 644125 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 30 | 10 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2875 | 865 | 865 | 548 | 1260 | 0.0039 | 0.0 | 724739 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2858 | 846 | 846 | 488 | 1240 | 0.0039 | 0.0 | 724525 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 25 | high | 0 | 0 | 0 | 0 | 6 | 4 | 0 | 6 | 0 | 0 | 6230 | 657 | 665 | 496 | 496 | 0.0040 | 0.0 | 641708 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3072 | 821 | 1061 | 540 | 1208 | 0.0726 | 0.0 | 634846 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2788 | 763 | 777 | 604 | 984 | 0.0039 | 0.0 | 631436 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2811 | 747 | 798 | 516 | 1268 | 0.0039 | 0.0 | 631942 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 810 | 810 | 712 | 1116 | 0.0039 | 0.0 | 633478 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2758 | 734 | 751 | 696 | 1080 | 0.0039 | 0.0 | 631448 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2802 | 757 | 785 | 476 | 940 | 0.0039 | 0.0 | 650079 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 35 | 3 | high | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3620 | 727 | 879 | 480 | 1656 | 0.0039 | 0.0 | 776934 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2847 | 837 | 837 | 660 | 1092 | 0.0277 | 0.0 | 642356 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2854 | 839 | 839 | 488 | 1140 | 0.0000 | 0.0 | 634450 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2785 | 757 | 775 | 492 | 928 | 0.0039 | 0.0 | 631219 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2707 | 673 | 695 | 528 | 944 | 0.0039 | 0.0 | 631143 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2824 | 814 | 814 | 616 | 1020 | 0.0095 | 0.0 | 633709 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2843 | 783 | 833 | 524 | 936 | 0.0044 | 0.0 | 631872 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2723 | 687 | 709 | 504 | 872 | 0.0039 | 0.0 | 632215 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2709 | 688 | 697 | 496 | 1136 | 0.0039 | 0.0 | 632747 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2704 | 692 | 692 | 520 | 828 | 0.0019 | 0.0 | 525768 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2755 | 744 | 744 | 476 | 860 | 0.0019 | 0.0 | 525614 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2675 | 665 | 666 | 544 | 840 | 0.0019 | 0.0 | 525740 |
| logged-in-reader | desktop | reading | /vi/reading/session/e8442542-d814-4e53-9ec0-89cda66adb6d | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2761 | 739 | 750 | 524 | 900 | 0.0039 | 0.0 | 632102 |
| logged-in-reader | desktop | reading | /vi/reading/session/a9542453-e3a0-4868-96e9-50386f83dd83 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2921 | 744 | 913 | 500 | 868 | 0.0039 | 0.0 | 725015 |
| logged-in-reader | desktop | reading | /vi/reading/session/0f340ee9-5a42-4a05-b944-f39173ddbdfb | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2727 | 703 | 716 | 536 | 904 | 0.0039 | 0.0 | 632045 |
| logged-in-reader | desktop | reading | /vi/reading/session/f6584958-4d5d-4678-9381-0f5886df197f | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2816 | 791 | 805 | 584 | 984 | 0.0039 | 0.0 | 631768 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2775 | 744 | 763 | 668 | 992 | 0.0039 | 0.0 | 631047 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2702 | 675 | 693 | 520 | 900 | 0.0039 | 0.0 | 630807 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2794 | 784 | 784 | 700 | 1084 | 0.0039 | 0.0 | 630959 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2690 | 663 | 678 | 524 | 1108 | 0.0039 | 0.0 | 632810 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2856 | 815 | 839 | 588 | 1244 | 0.0039 | 0.0 | 632670 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2908 | 881 | 898 | 828 | 1452 | 0.0039 | 0.0 | 632758 |
| logged-out | mobile | auth-public | /vi | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3529 | 1290 | 1522 | 944 | 1304 | 0.0000 | 0.0 | 602181 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2695 | 687 | 687 | 568 | 568 | 0.0000 | 0.0 | 511746 |
| logged-out | mobile | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2662 | 652 | 652 | 468 | 468 | 0.0000 | 0.0 | 512847 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2691 | 683 | 683 | 468 | 468 | 0.0000 | 0.0 | 511292 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2837 | 825 | 825 | 604 | 604 | 0.0000 | 0.0 | 511406 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2785 | 509 | 774 | 524 | 524 | 0.0000 | 0.0 | 511408 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2963 | 952 | 952 | 700 | 700 | 0.0000 | 0.0 | 525324 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2723 | 711 | 711 | 456 | 772 | 0.0000 | 0.0 | 525263 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2824 | 811 | 811 | 488 | 796 | 0.0000 | 0.0 | 525386 |
| logged-in-admin | mobile | auth-public | /vi | 34 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2941 | 805 | 929 | 516 | 872 | 0.0032 | 0.0 | 609946 |
| logged-in-admin | mobile | reading | /vi/reading | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2918 | 875 | 904 | 524 | 884 | 0.0000 | 0.0 | 642052 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 30 | 9 | high | 0 | 0 | 0 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 2925 | 885 | 915 | 524 | 1164 | 0.0000 | 0.0 | 644045 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 30 | 11 | high | 0 | 0 | 0 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2894 | 881 | 881 | 464 | 1120 | 0.0000 | 0.0 | 724275 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2929 | 919 | 919 | 492 | 880 | 0.0000 | 0.0 | 725578 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 27 | high | 0 | 0 | 0 | 0 | 14 | 4 | 3 | 13 | 1 | 0 | 5771 | 816 | 870 | 484 | 820 | 0.0000 | 0.0 | 642629 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3039 | 723 | 1028 | 492 | 828 | 0.0000 | 0.0 | 634264 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2768 | 733 | 757 | 636 | 964 | 0.0000 | 0.0 | 631057 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2824 | 872 | 1139 | 656 | 1016 | 0.0760 | 0.0 | 630298 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2984 | 971 | 971 | 524 | 1168 | 0.0000 | 0.0 | 634099 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3898 | 1190 | 1886 | 920 | 920 | 0.0071 | 0.0 | 631161 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3246 | 1134 | 1236 | 944 | 944 | 0.0000 | 0.0 | 649877 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 36 | 3 | critical | 0 | 0 | 2 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3935 | 1009 | 1195 | 704 | 2156 | 0.0051 | 0.0 | 777655 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 32 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3474 | 1438 | 1438 | 748 | 1264 | 0.0071 | 0.0 | 645119 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3476 | 1419 | 1468 | 936 | 1288 | 0.0071 | 0.0 | 633785 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2880 | 829 | 866 | 496 | 840 | 0.0000 | 0.0 | 631848 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3049 | 959 | 1039 | 852 | 852 | 0.0000 | 0.0 | 631643 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 59 | 3 | critical | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4718 | 1160 | 1194 | 648 | 1388 | 0.0071 | 0.0 | 1254564 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2838 | 804 | 826 | 576 | 916 | 0.0000 | 0.0 | 631632 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2759 | 700 | 724 | 484 | 812 | 0.0000 | 0.0 | 632088 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2777 | 740 | 768 | 484 | 820 | 0.0000 | 0.0 | 632343 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2753 | 738 | 738 | 496 | 828 | 0.0032 | 0.0 | 525660 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2786 | 775 | 776 | 492 | 800 | 0.0032 | 0.0 | 525581 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 734 | 734 | 516 | 852 | 0.0032 | 0.0 | 525571 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2791 | 745 | 778 | 468 | 796 | 0.0000 | 0.0 | 646917 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2752 | 708 | 739 | 472 | 796 | 0.0000 | 0.0 | 646866 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2808 | 777 | 797 | 472 | 780 | 0.0000 | 0.0 | 645411 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2965 | 841 | 956 | 504 | 836 | 0.0000 | 0.0 | 697628 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2834 | 789 | 825 | 504 | 828 | 0.0000 | 0.0 | 644143 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2791 | 741 | 779 | 476 | 796 | 0.0000 | 0.0 | 645897 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2806 | 791 | 791 | 516 | 868 | 0.0000 | 0.0 | 648308 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2890 | 766 | 878 | 444 | 804 | 0.0000 | 0.0 | 688709 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2922 | 902 | 908 | 504 | 1172 | 0.0000 | 0.0 | 649439 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2922 | 878 | 912 | 544 | 880 | 0.0000 | 0.0 | 645575 |
| logged-in-admin | mobile | reading | /vi/reading/session/dc1500b6-435d-4af0-92f4-bd3318a2ce9a | 28 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 720 | 741 | 464 | 812 | 0.0000 | 0.0 | 631702 |
| logged-in-admin | mobile | reading | /vi/reading/session/8d742e33-9bf0-462c-b6c3-4402e7681c13 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2960 | 773 | 947 | 484 | 824 | 0.0000 | 0.0 | 692616 |
| logged-in-admin | mobile | reading | /vi/reading/session/2eaccbea-6a57-40ee-bd93-314cef2e7685 | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2772 | 731 | 759 | 488 | 824 | 0.0000 | 0.0 | 631699 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2743 | 709 | 732 | 456 | 788 | 0.0000 | 0.0 | 630703 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2845 | 764 | 830 | 484 | 836 | 0.0000 | 0.0 | 630634 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2992 | 951 | 977 | 596 | 948 | 0.0000 | 0.0 | 631529 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2918 | 896 | 906 | 524 | 888 | 0.0000 | 0.0 | 632243 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2857 | 821 | 848 | 496 | 928 | 0.0000 | 0.0 | 632232 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2863 | 800 | 853 | 512 | 868 | 0.0000 | 0.0 | 632744 |
| logged-in-reader | mobile | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2878 | 853 | 867 | 640 | 1008 | 0.0028 | 0.0 | 537130 |
| logged-in-reader | mobile | reading | /vi/reading | 30 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2872 | 823 | 852 | 444 | 828 | 0.0000 | 0.0 | 643174 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 31 | 8 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2891 | 871 | 872 | 488 | 1124 | 0.0000 | 0.0 | 644668 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 35 | 6 | high | 0 | 0 | 0 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2954 | 837 | 942 | 444 | 852 | 0.0069 | 0.0 | 796615 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2879 | 864 | 864 | 472 | 1140 | 0.0000 | 0.0 | 725538 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 25 | high | 0 | 0 | 0 | 0 | 6 | 2 | 0 | 6 | 0 | 0 | 5585 | 693 | 707 | 456 | 456 | 0.0000 | 0.0 | 641802 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3037 | 866 | 1025 | 456 | 1092 | 0.0821 | 0.0 | 634800 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 760 | 807 | 464 | 804 | 0.0000 | 0.0 | 631325 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2814 | 784 | 801 | 616 | 964 | 0.0000 | 0.0 | 631917 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 30 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2918 | 902 | 903 | 500 | 1132 | 0.0000 | 0.0 | 635251 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2754 | 727 | 741 | 500 | 824 | 0.0000 | 0.0 | 631391 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2849 | 787 | 818 | 508 | 856 | 0.0000 | 0.0 | 650092 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 35 | 3 | high | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3657 | 815 | 915 | 484 | 1608 | 0.0051 | 0.0 | 776966 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2881 | 863 | 863 | 476 | 868 | 0.0000 | 0.0 | 643161 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2874 | 859 | 859 | 448 | 1100 | 0.0000 | 0.0 | 634508 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2850 | 798 | 835 | 468 | 832 | 0.0000 | 0.0 | 631043 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2847 | 829 | 838 | 476 | 828 | 0.0000 | 0.0 | 631472 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 30 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2947 | 901 | 901 | 504 | 1144 | 0.0330 | 0.0 | 634276 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 30 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2888 | 873 | 873 | 480 | 824 | 0.0000 | 0.0 | 633513 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2816 | 794 | 802 | 460 | 796 | 0.0000 | 0.0 | 632324 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2812 | 729 | 800 | 456 | 808 | 0.0000 | 0.0 | 632689 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2775 | 760 | 761 | 472 | 800 | 0.0028 | 0.0 | 525568 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2698 | 684 | 684 | 440 | 748 | 0.0028 | 0.0 | 525719 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2732 | 715 | 715 | 460 | 784 | 0.0028 | 0.0 | 525753 |
| logged-in-reader | mobile | reading | /vi/reading/session/09f5d83e-ad26-4930-8cbb-13ce88c66000 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2946 | 767 | 933 | 476 | 824 | 0.0000 | 0.0 | 693217 |
| logged-in-reader | mobile | reading | /vi/reading/session/70126a07-9e85-4898-8b64-061f6d14e550 | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2982 | 851 | 970 | 508 | 880 | 0.0000 | 0.0 | 692715 |
| logged-in-reader | mobile | reading | /vi/reading/session/173188c4-b981-4fe0-a3d4-0c0fe9946027 | 28 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2763 | 733 | 751 | 468 | 800 | 0.0000 | 0.0 | 631995 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2828 | 806 | 815 | 484 | 812 | 0.0000 | 0.0 | 630584 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2904 | 858 | 887 | 524 | 852 | 0.0000 | 0.0 | 631481 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2804 | 738 | 790 | 456 | 780 | 0.0000 | 0.0 | 631070 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2839 | 814 | 823 | 480 | 828 | 0.0000 | 0.0 | 632328 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2773 | 745 | 757 | 492 | 848 | 0.0000 | 0.0 | 632390 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2803 | 735 | 792 | 452 | 800 | 0.0000 | 0.0 | 632717 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 13 | 4 | 3 | 12 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 6 | 4 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 14 | 4 | 3 | 13 | 1 | 0 |
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
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/dc12ad57-48d6-4e3f-bfa2-8d84f255a2d2 | 34 | high | 2 | 30 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/174febce-76ad-4bac-bfd9-7471984a6b35 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/44f456bd-915f-4f30-ba10-7b3bac6cf8f5 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/8bf24139-8c59-4fa5-bbdb-477161895ddb | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/e8442542-d814-4e53-9ec0-89cda66adb6d | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/a9542453-e3a0-4868-96e9-50386f83dd83 | 34 | high | 2 | 30 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/0f340ee9-5a42-4a05-b944-f39173ddbdfb | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/f6584958-4d5d-4678-9381-0f5886df197f | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 34 | high | 4 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 32 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 59 | critical | 2 | 53 | 0 |
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
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/dc1500b6-435d-4af0-92f4-bd3318a2ce9a | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/8d742e33-9bf0-462c-b6c3-4402e7681c13 | 33 | high | 2 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/2eaccbea-6a57-40ee-bd93-314cef2e7685 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 35 | high | 0 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/09f5d83e-ad26-4930-8cbb-13ce88c66000 | 33 | high | 2 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/70126a07-9e85-4898-8b64-061f6d14e550 | 33 | high | 2 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/173188c4-b981-4fe0-a3d4-0c0fe9946027 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 1303 | 187 | static | https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1270 | 587 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 1239 | 777 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 1222 | 330 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 1221 | 366 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 1159 | 189 | static | https://www.tarotnow.xyz/_next/static/chunks/134bl8i0js1.i.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 1111 | 593 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 1065 | 389 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1056 | 571 | static | https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 1041 | 185 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1017 | 635 | html | https://www.tarotnow.xyz/vi |
| logged-out | desktop | auth-public | /vi | GET | 200 | 991 | 585 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqe0oax2~t9t.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 941 | 379 | static | https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 928 | 380 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-out | desktop | auth-public | /vi/login | GET | 200 | 894 | 573 | html | https://www.tarotnow.xyz/vi/login |
| logged-out | mobile | auth-public | /vi/legal/tos | GET | 200 | 894 | 547 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 884 | 378 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 882 | 381 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 881 | 475 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 872 | 183 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 871 | 407 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | GET | 200 | 870 | 368 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 869 | 860 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 868 | 334 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 865 | 347 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 857 | 352 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 849 | 330 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | admin | /vi/admin/users | GET | 200 | 845 | 386 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 837 | 339 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 829 | 333 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 827 | 338 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 827 | 337 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 826 | 376 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 825 | 324 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 819 | 343 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 817 | 329 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 814 | 553 | static | https://www.tarotnow.xyz/_next/static/chunks/0uvdfew0zvv3h.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 812 | 329 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 811 | 325 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 811 | 333 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 810 | 330 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 806 | 294 | static | https://www.tarotnow.xyz/_next/static/chunks/023q4p3fs49f5.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 805 | 322 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 804 | 328 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 804 | 317 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 797 | 322 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | auth-public | /vi | GET | 200 | 794 | 451 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 793 | 329 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 790 | 327 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 789 | 543 | static | https://www.tarotnow.xyz/_next/static/chunks/0xl.ndi-x969l.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 788 | 303 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 788 | 292 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 788 | 310 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 786 | 351 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | GET | 200 | 785 | 316 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-reader | mobile | reading | /vi/reading/session/70126a07-9e85-4898-8b64-061f6d14e550 | GET | 200 | 785 | 334 | html | https://www.tarotnow.xyz/vi/reading/session/70126a07-9e85-4898-8b64-061f6d14e550 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 784 | 322 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 782 | 311 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 781 | 352 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 779 | 330 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 778 | 303 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 776 | 314 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 775 | 347 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 774 | 316 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 772 | 322 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 772 | 310 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 772 | 381 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 772 | 313 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 771 | 312 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 771 | 306 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 771 | 569 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 770 | 305 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 770 | 315 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 770 | 315 | static | https://www.tarotnow.xyz/_next/static/chunks/134bl8i0js1.i.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 769 | 318 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 768 | 334 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 767 | 262 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 767 | 328 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 765 | 307 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 762 | 314 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | auth-public | /vi | GET | 200 | 761 | 354 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 761 | 295 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 759 | 293 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 757 | 336 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 756 | 588 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 752 | 402 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 750 | 318 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 749 | 315 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 749 | 351 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 747 | 327 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 747 | 337 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 744 | 311 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | admin | /vi/admin/promotions | GET | 200 | 744 | 332 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 743 | 366 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | - | 740 | - | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 739 | 374 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 737 | 407 | static | https://www.tarotnow.xyz/_next/static/chunks/134bl8i0js1.i.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 736 | 337 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 734 | 341 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 733 | 234 | static | https://www.tarotnow.xyz/_next/static/chunks/0eqe0oax2~t9t.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 733 | 320 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-reader | desktop | reading | /vi/reading/session/f6584958-4d5d-4678-9381-0f5886df197f | GET | 200 | 730 | 399 | html | https://www.tarotnow.xyz/vi/reading/session/f6584958-4d5d-4678-9381-0f5886df197f |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 729 | 333 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | mobile | reading | /vi/reading/session/8d742e33-9bf0-462c-b6c3-4402e7681c13 | GET | 200 | 728 | 332 | html | https://www.tarotnow.xyz/vi/reading/session/8d742e33-9bf0-462c-b6c3-4402e7681c13 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 726 | 336 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 726 | 337 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 726 | 329 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 724 | 536 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 723 | 333 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 722 | 321 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 717 | 315 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | mobile | reading | /vi/reading/session/09f5d83e-ad26-4930-8cbb-13ce88c66000 | GET | 200 | 716 | 330 | html | https://www.tarotnow.xyz/vi/reading/session/09f5d83e-ad26-4930-8cbb-13ce88c66000 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 715 | 324 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 714 | 336 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 711 | 317 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 709 | 316 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 705 | 316 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-reader | desktop | reading | /vi/reading/session/e8442542-d814-4e53-9ec0-89cda66adb6d | GET | 200 | 705 | 324 | html | https://www.tarotnow.xyz/vi/reading/session/e8442542-d814-4e53-9ec0-89cda66adb6d |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | GET | 200 | 705 | 324 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 704 | 318 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 703 | 450 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-out | mobile | auth-public | /vi/reset-password | GET | 200 | 702 | 443 | html | https://www.tarotnow.xyz/vi/reset-password |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 701 | 312 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 699 | 338 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | GET | 200 | 698 | 335 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| - | - | - | - |

## Coverage Notes
| Scenario | Viewport | Note |
| --- | --- | --- |
| logged-out | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-out | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-out | desktop | dynamic-routes: skipped for logged-out scenario. |
| logged-out | desktop | scenario-filter:logged-out-protected-routes-skipped=30 |
| logged-in-admin | desktop | origin-discovery:/sitemap.xml:routes-22 |
| logged-in-admin | desktop | origin-discovery:/robots.txt:routes-22 |
| logged-in-admin | desktop | reading.init.daily_1: created dc12ad57-48d6-4e3f-bfa2-8d84f255a2d2. |
| logged-in-admin | desktop | reading.init.spread_3: created 174febce-76ad-4bac-bfd9-7471984a6b35. |
| logged-in-admin | desktop | reading.init.spread_5: created 44f456bd-915f-4f30-ba10-7b3bac6cf8f5. |
| logged-in-admin | desktop | reading.init.spread_10: created 8bf24139-8c59-4fa5-bbdb-477161895ddb. |
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
| logged-in-reader | desktop | reading.init.daily_1: created e8442542-d814-4e53-9ec0-89cda66adb6d. |
| logged-in-reader | desktop | reading.init.spread_3: created a9542453-e3a0-4868-96e9-50386f83dd83. |
| logged-in-reader | desktop | reading.init.spread_5: created 0f340ee9-5a42-4a05-b944-f39173ddbdfb. |
| logged-in-reader | desktop | reading.init.spread_10: created f6584958-4d5d-4678-9381-0f5886df197f. |
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
| logged-in-admin | mobile | reading.init.spread_3: created dc1500b6-435d-4af0-92f4-bd3318a2ce9a. |
| logged-in-admin | mobile | reading.init.spread_5: created 8d742e33-9bf0-462c-b6c3-4402e7681c13. |
| logged-in-admin | mobile | reading.init.spread_10: created 2eaccbea-6a57-40ee-bd93-314cef2e7685. |
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
| logged-in-reader | mobile | reading.init.spread_3: created 09f5d83e-ad26-4930-8cbb-13ce88c66000. |
| logged-in-reader | mobile | reading.init.spread_5: created 70126a07-9e85-4898-8b64-061f6d14e550. |
| logged-in-reader | mobile | reading.init.spread_10: created 173188c4-b981-4fe0-a3d4-0c0fe9946027. |
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
