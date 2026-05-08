# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T19:44:52.162Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 5
- High pages (request count): 142
- High slow requests: 35
- Medium slow requests: 183

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2853 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 2982 | 1283 | 0 | 0 | 17 | 0 | yes |
| logged-in-reader | desktop | 33 | 2966 | 978 | 0 | 0 | 16 | 1 | yes |
| logged-out | mobile | 9 | 2762 | 226 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 2890 | 1254 | 0 | 0 | 14 | 0 | yes |
| logged-in-reader | mobile | 33 | 2932 | 983 | 0 | 0 | 17 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.3 | 2897 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2701 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6171 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4118 | 0 |
| logged-in-admin | desktop | gacha | 2 | 33.0 | 2904 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 3297 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2870 | 0 |
| logged-in-admin | desktop | inventory | 1 | 32.0 | 2879 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3228 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2707 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2701 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.3 | 2817 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2724 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.3 | 2803 | 0 |
| logged-in-admin | desktop | reading | 5 | 31.0 | 2840 | 0 |
| logged-in-admin | desktop | wallet | 4 | 35.3 | 3092 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.2 | 2764 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2760 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5490 | 0 |
| logged-in-admin | mobile | community | 1 | 36.0 | 3670 | 0 |
| logged-in-admin | mobile | gacha | 2 | 32.0 | 2867 | 0 |
| logged-in-admin | mobile | gamification | 1 | 32.0 | 2896 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2642 | 0 |
| logged-in-admin | mobile | inventory | 1 | 32.0 | 2847 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 32.0 | 2886 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2661 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2734 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.3 | 3080 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2732 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2777 | 0 |
| logged-in-admin | mobile | reading | 5 | 30.4 | 2920 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.5 | 2740 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2840 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6259 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3486 | 0 |
| logged-in-reader | desktop | gacha | 2 | 31.5 | 2859 | 0 |
| logged-in-reader | desktop | gamification | 1 | 31.0 | 2910 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2727 | 0 |
| logged-in-reader | desktop | inventory | 1 | 32.0 | 2880 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 33.0 | 2876 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2783 | 0 |
| logged-in-reader | desktop | notifications | 1 | 30.0 | 2875 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.7 | 2886 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2699 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.4 | 2783 | 0 |
| logged-in-reader | desktop | reading | 5 | 28.6 | 2774 | 0 |
| logged-in-reader | desktop | wallet | 4 | 35.5 | 3063 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2707 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5560 | 0 |
| logged-in-reader | mobile | community | 1 | 36.0 | 3659 | 0 |
| logged-in-reader | mobile | gacha | 2 | 31.5 | 2877 | 0 |
| logged-in-reader | mobile | gamification | 1 | 32.0 | 2899 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2703 | 0 |
| logged-in-reader | mobile | inventory | 1 | 30.0 | 2854 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 33.0 | 2845 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2691 | 0 |
| logged-in-reader | mobile | notifications | 1 | 31.0 | 2867 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.3 | 2849 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2778 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.0 | 2788 | 0 |
| logged-in-reader | mobile | reading | 5 | 28.6 | 2754 | 0 |
| logged-in-reader | mobile | wallet | 4 | 36.3 | 3059 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2705 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4062 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2697 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2684 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3327 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2703 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4062 | 1747 | 2050 | 1532 | 1532 | 0.0000 | 367.0 | 600941 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2723 | 715 | 715 | 676 | 676 | 0.0000 | 0.0 | 511833 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2643 | 636 | 636 | 512 | 512 | 0.0000 | 0.0 | 512466 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2710 | 705 | 705 | 484 | 484 | 0.0000 | 0.0 | 511513 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2732 | 727 | 727 | 560 | 560 | 0.0000 | 0.0 | 511604 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2716 | 708 | 709 | 484 | 484 | 0.0000 | 0.0 | 511734 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2724 | 710 | 711 | 620 | 936 | 0.0000 | 0.0 | 525362 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2682 | 671 | 671 | 548 | 872 | 0.0000 | 0.0 | 525590 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2686 | 673 | 673 | 560 | 884 | 0.0000 | 0.0 | 525560 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2870 | 844 | 863 | 804 | 1348 | 0.0035 | 191.0 | 537128 |
| logged-in-admin | desktop | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2942 | 899 | 935 | 544 | 980 | 0.0041 | 0.0 | 644751 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 1 | 0 | 5 | 0 | 5 | 5 | 0 | 0 | 2879 | 871 | 873 | 520 | 1180 | 0.0041 | 0.0 | 646394 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 33 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2901 | 894 | 894 | 520 | 1184 | 0.0041 | 0.0 | 727731 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2907 | 897 | 897 | 520 | 1148 | 0.0041 | 0.0 | 728895 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 28 | high | 0 | 0 | 0 | 0 | 13 | 4 | 4 | 12 | 1 | 0 | 6171 | 757 | 775 | 628 | 628 | 0.0042 | 0.0 | 643040 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2982 | 867 | 979 | 528 | 1136 | 0.0489 | 0.0 | 637122 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2788 | 773 | 789 | 568 | 924 | 0.0041 | 0.0 | 631299 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 40 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2681 | 860 | 989 | 532 | 908 | 0.0489 | 0.0 | 630176 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2877 | 874 | 874 | 512 | 1104 | 0.0041 | 0.0 | 635777 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2701 | 693 | 697 | 528 | 916 | 0.0041 | 0.0 | 631423 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3228 | 889 | 1225 | 520 | 1176 | 0.0041 | 0.0 | 651894 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | 5 | high | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 4118 | 916 | 1363 | 640 | 2224 | 0.0041 | 0.0 | 777374 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | 7 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3297 | 863 | 1293 | 520 | 940 | 0.0279 | 0.0 | 643635 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 55 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3765 | 1012 | 1013 | 848 | 1424 | 0.0000 | 37.0 | 1135059 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2817 | 802 | 812 | 640 | 1020 | 0.0041 | 0.0 | 631389 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2896 | 832 | 883 | 536 | 1076 | 0.0041 | 0.0 | 634192 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2888 | 864 | 864 | 520 | 1208 | 0.0041 | 0.0 | 630647 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2701 | 693 | 694 | 524 | 892 | 0.0046 | 0.0 | 631642 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2724 | 700 | 715 | 532 | 924 | 0.0041 | 0.0 | 631888 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2686 | 666 | 678 | 516 | 1100 | 0.0041 | 0.0 | 632606 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2747 | 742 | 742 | 520 | 868 | 0.0020 | 0.0 | 525842 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2725 | 705 | 705 | 492 | 820 | 0.0020 | 0.0 | 525797 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2648 | 642 | 642 | 492 | 784 | 0.0020 | 0.0 | 525797 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3240 | 768 | 1229 | 536 | 908 | 0.0000 | 0.0 | 647127 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2893 | 884 | 886 | 740 | 1164 | 0.0000 | 0.0 | 647286 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 757 | 761 | 520 | 864 | 0.0000 | 0.0 | 645550 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3268 | 778 | 1262 | 528 | 864 | 0.0022 | 0.0 | 697957 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2835 | 808 | 826 | 584 | 916 | 0.0000 | 0.0 | 644214 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2919 | 829 | 911 | 548 | 864 | 0.0000 | 0.0 | 647711 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2713 | 688 | 705 | 500 | 1016 | 0.0000 | 0.0 | 648433 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 735 | 747 | 512 | 1024 | 0.0000 | 0.0 | 655642 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2833 | 824 | 825 | 732 | 1060 | 0.0000 | 0.0 | 649617 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2740 | 720 | 729 | 528 | 864 | 0.0000 | 0.0 | 645844 |
| logged-in-admin | desktop | reading | /vi/reading/session/66977a6c-16e2-42b4-9ce0-b7af67b77dff | 33 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2879 | 771 | 867 | 880 | 880 | 0.0054 | 0.0 | 714610 |
| logged-in-admin | desktop | reading | /vi/reading/session/3ed2c65a-3f97-459c-b081-89ac900bd3ad | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2739 | 723 | 732 | 496 | 892 | 0.0041 | 0.0 | 631830 |
| logged-in-admin | desktop | reading | /vi/reading/session/406f0096-c2fa-4796-a9f1-a2510cc0e200 | 35 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2953 | 753 | 935 | 504 | 908 | 0.0041 | 0.0 | 726161 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2748 | 721 | 736 | 536 | 908 | 0.0041 | 0.0 | 630733 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2838 | 813 | 831 | 600 | 992 | 0.0041 | 0.0 | 630978 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2815 | 791 | 804 | 560 | 964 | 0.0041 | 0.0 | 630722 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2759 | 741 | 750 | 688 | 1324 | 0.0041 | 0.0 | 632588 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2763 | 740 | 755 | 516 | 1176 | 0.0041 | 0.0 | 632524 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2821 | 778 | 814 | 500 | 1080 | 0.0041 | 0.0 | 632565 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2727 | 710 | 710 | 584 | 1164 | 0.0033 | 234.0 | 537348 |
| logged-in-reader | desktop | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2909 | 863 | 899 | 508 | 960 | 0.0039 | 0.0 | 645007 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2880 | 871 | 871 | 516 | 1208 | 0.0039 | 0.0 | 646604 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2888 | 879 | 879 | 528 | 1192 | 0.0039 | 0.0 | 728052 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2830 | 819 | 819 | 728 | 1364 | 0.0039 | 0.0 | 723944 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 26 | high | 0 | 0 | 0 | 0 | 6 | 4 | 0 | 6 | 0 | 0 | 6259 | 684 | 709 | 508 | 508 | 0.0040 | 0.0 | 641870 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3061 | 901 | 1051 | 608 | 1004 | 0.0726 | 0.0 | 638474 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2873 | 801 | 855 | 568 | 932 | 0.0039 | 0.0 | 631278 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2723 | 700 | 714 | 548 | 1352 | 0.0039 | 0.0 | 631872 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2858 | 847 | 847 | 488 | 1156 | 0.0039 | 0.0 | 636724 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 765 | 831 | 504 | 948 | 0.0039 | 0.0 | 631475 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2876 | 868 | 868 | 516 | 1172 | 0.0039 | 0.0 | 654141 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3486 | 721 | 753 | 528 | 1724 | 0.0039 | 0.0 | 642998 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2910 | 868 | 896 | 500 | 944 | 0.0277 | 0.0 | 644676 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 31 | 4 | high | 0 | 0 | 2 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 2873 | 864 | 864 | 524 | 1252 | 0.0039 | 0.0 | 636200 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 55 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3779 | 1075 | 1075 | 824 | 1436 | 0.0000 | 33.0 | 1134265 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2735 | 698 | 726 | 504 | 876 | 0.0039 | 0.0 | 631299 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2866 | 847 | 847 | 552 | 944 | 0.0095 | 0.0 | 632830 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2875 | 864 | 864 | 540 | 920 | 0.0040 | 0.0 | 634214 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2699 | 663 | 689 | 496 | 872 | 0.0039 | 0.0 | 632287 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2723 | 688 | 714 | 512 | 1064 | 0.0039 | 0.0 | 632669 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2681 | 671 | 671 | 524 | 892 | 0.0019 | 0.0 | 525861 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2960 | 656 | 953 | 544 | 876 | 0.0019 | 0.0 | 525836 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2708 | 697 | 698 | 492 | 808 | 0.0019 | 0.0 | 525985 |
| logged-in-reader | desktop | reading | /vi/reading/session/aa6ce66a-4820-4513-b383-a071f3d5b7e2 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2748 | 707 | 736 | 544 | 952 | 0.0039 | 0.0 | 631962 |
| logged-in-reader | desktop | reading | /vi/reading/session/c48ac3bf-3318-46d9-ad10-d83375060178 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2745 | 708 | 734 | 508 | 916 | 0.0039 | 0.0 | 631945 |
| logged-in-reader | desktop | reading | /vi/reading/session/9386ba2f-af68-44b8-a7d6-689e2ff5c809 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 715 | 735 | 508 | 912 | 0.0039 | 0.0 | 632019 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2755 | 729 | 747 | 532 | 884 | 0.0039 | 0.0 | 630906 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2766 | 738 | 756 | 548 | 908 | 0.0039 | 0.0 | 631056 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 705 | 734 | 540 | 900 | 0.0039 | 0.0 | 631055 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2774 | 740 | 764 | 692 | 1236 | 0.0039 | 0.0 | 632709 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2732 | 696 | 721 | 496 | 1004 | 0.0039 | 0.0 | 632820 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2851 | 788 | 838 | 532 | 1084 | 0.0039 | 0.0 | 633000 |
| logged-out | mobile | auth-public | /vi | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3327 | 1185 | 1319 | 844 | 1192 | 0.0000 | 0.0 | 602261 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2706 | 645 | 697 | 596 | 596 | 0.0000 | 0.0 | 511919 |
| logged-out | mobile | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2625 | 614 | 614 | 448 | 448 | 0.0000 | 0.0 | 513045 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2719 | 709 | 709 | 464 | 464 | 0.0000 | 0.0 | 511436 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2719 | 708 | 708 | 512 | 512 | 0.0000 | 0.0 | 511584 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2652 | 643 | 643 | 444 | 444 | 0.0000 | 0.0 | 511587 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2655 | 640 | 641 | 432 | 744 | 0.0000 | 0.0 | 525494 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2739 | 727 | 727 | 508 | 820 | 0.0000 | 0.0 | 525410 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2716 | 701 | 701 | 460 | 460 | 0.0000 | 0.0 | 525553 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2642 | 631 | 632 | 424 | 772 | 0.0032 | 0.0 | 537259 |
| logged-in-admin | mobile | reading | /vi/reading | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3255 | 1238 | 1245 | 748 | 1256 | 0.0071 | 0.0 | 641562 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 32 | 8 | high | 0 | 0 | 1 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 2847 | 836 | 836 | 456 | 1092 | 0.0000 | 0.0 | 646465 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 33 | 9 | high | 0 | 0 | 2 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2904 | 888 | 889 | 532 | 1188 | 0.0000 | 0.0 | 727988 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2830 | 816 | 816 | 424 | 900 | 0.0000 | 0.0 | 726811 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 27 | high | 0 | 0 | 0 | 0 | 13 | 4 | 4 | 12 | 1 | 0 | 5490 | 712 | 712 | 432 | 432 | 0.0000 | 0.0 | 642782 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3706 | 1417 | 1697 | 432 | 1424 | 0.0760 | 0.0 | 637127 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2791 | 740 | 782 | 444 | 776 | 0.0000 | 0.0 | 631360 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2744 | 833 | 1045 | 592 | 936 | 0.0760 | 0.0 | 630501 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2889 | 854 | 854 | 436 | 1108 | 0.0000 | 0.0 | 635475 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2760 | 736 | 741 | 496 | 844 | 0.0000 | 0.0 | 631339 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2886 | 858 | 858 | 492 | 1108 | 0.0000 | 0.0 | 652714 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 36 | 3 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 3670 | 814 | 933 | 488 | 1856 | 0.0051 | 0.0 | 778285 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 32 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2896 | 869 | 869 | 440 | 848 | 0.0000 | 0.0 | 645576 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2844 | 833 | 833 | 432 | 1104 | 0.0000 | 0.0 | 635773 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2705 | 685 | 695 | 532 | 868 | 0.0000 | 0.0 | 631200 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2669 | 654 | 659 | 420 | 828 | 0.0000 | 0.0 | 631843 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 40 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2742 | 807 | 810 | 504 | 848 | 0.0071 | 0.0 | 630725 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2734 | 714 | 724 | 468 | 816 | 0.0000 | 0.0 | 631700 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2732 | 707 | 708 | 496 | 832 | 0.0000 | 0.0 | 631926 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2851 | 829 | 838 | 456 | 832 | 0.0000 | 0.0 | 632467 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2646 | 635 | 635 | 416 | 732 | 0.0032 | 0.0 | 525846 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2676 | 665 | 665 | 448 | 776 | 0.0032 | 0.0 | 525764 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2660 | 650 | 650 | 424 | 764 | 0.0032 | 0.0 | 525955 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2718 | 699 | 705 | 520 | 836 | 0.0000 | 0.0 | 647148 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2776 | 765 | 765 | 508 | 836 | 0.0000 | 0.0 | 647301 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2712 | 691 | 699 | 432 | 748 | 0.0000 | 0.0 | 645437 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 32 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 784 | 854 | 476 | 812 | 0.0000 | 0.0 | 697934 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2711 | 696 | 698 | 420 | 728 | 0.0000 | 0.0 | 644246 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2783 | 758 | 771 | 464 | 804 | 0.0000 | 0.0 | 646098 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2728 | 713 | 713 | 452 | 788 | 0.0000 | 0.0 | 648402 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 28 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2740 | 716 | 731 | 444 | 812 | 0.0000 | 0.0 | 655863 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 803 | 806 | 652 | 988 | 0.0000 | 0.0 | 649616 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2786 | 769 | 774 | 488 | 796 | 0.0000 | 0.0 | 645651 |
| logged-in-admin | mobile | reading | /vi/reading/session/0d0d4f30-4b83-4ad4-9fe0-4b955a106f7b | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2887 | 721 | 879 | 428 | 764 | 0.0000 | 0.0 | 694412 |
| logged-in-admin | mobile | reading | /vi/reading/session/0047a5d6-a6e2-46de-8509-9d535afe7580 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2884 | 760 | 875 | 424 | 776 | 0.0000 | 0.0 | 694539 |
| logged-in-admin | mobile | reading | /vi/reading/session/f20fe7e3-4f3e-4ca7-b7e6-5b4fd40e6ab3 | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2725 | 707 | 710 | 452 | 784 | 0.0000 | 0.0 | 632014 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2771 | 717 | 762 | 432 | 752 | 0.0000 | 0.0 | 630665 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2774 | 758 | 765 | 552 | 888 | 0.0000 | 0.0 | 631003 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2780 | 729 | 770 | 452 | 804 | 0.0000 | 0.0 | 630947 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2715 | 700 | 702 | 448 | 848 | 0.0000 | 0.0 | 632595 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 714 | 718 | 484 | 836 | 0.0000 | 0.0 | 632508 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2780 | 758 | 766 | 640 | 984 | 0.0000 | 0.0 | 632707 |
| logged-in-reader | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2703 | 689 | 690 | 468 | 824 | 0.0032 | 0.0 | 537222 |
| logged-in-reader | mobile | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2865 | 821 | 843 | 428 | 820 | 0.0000 | 0.0 | 644547 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 30 | 10 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2854 | 839 | 839 | 480 | 1120 | 0.0000 | 0.0 | 644365 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 30 | 12 | high | 0 | 0 | 0 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2888 | 873 | 874 | 460 | 1124 | 0.0000 | 0.0 | 724514 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2865 | 851 | 851 | 464 | 1124 | 0.0000 | 0.0 | 727105 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 26 | high | 0 | 0 | 0 | 0 | 6 | 1 | 0 | 6 | 0 | 0 | 5560 | 726 | 739 | 468 | 468 | 0.0000 | 0.0 | 641826 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3126 | 881 | 1116 | 484 | 1132 | 0.0821 | 0.0 | 636132 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2707 | 683 | 694 | 452 | 776 | 0.0000 | 0.0 | 631574 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2713 | 686 | 701 | 432 | 768 | 0.0000 | 0.0 | 632249 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2876 | 861 | 861 | 532 | 868 | 0.0000 | 0.0 | 633631 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2707 | 677 | 693 | 444 | 768 | 0.0000 | 0.0 | 631652 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2845 | 831 | 832 | 424 | 1060 | 0.0000 | 0.0 | 654041 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 36 | 3 | critical | 0 | 0 | 1 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3659 | 766 | 917 | 472 | 1612 | 0.0051 | 0.0 | 778545 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 32 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2899 | 875 | 875 | 456 | 848 | 0.0000 | 0.0 | 645443 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2843 | 830 | 831 | 452 | 1100 | 0.0000 | 0.0 | 637147 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 55 | 5 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3792 | 1061 | 1061 | 804 | 1168 | 0.0000 | 0.0 | 1134223 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 708 | 717 | 456 | 792 | 0.0000 | 0.0 | 631469 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 31 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2871 | 857 | 857 | 448 | 1076 | 0.0330 | 0.0 | 635954 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 851 | 851 | 464 | 808 | 0.0000 | 0.0 | 635357 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2778 | 711 | 766 | 432 | 760 | 0.0000 | 0.0 | 632304 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 701 | 717 | 440 | 788 | 0.0000 | 0.0 | 632463 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2715 | 702 | 702 | 452 | 764 | 0.0032 | 0.0 | 525779 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2633 | 617 | 617 | 432 | 740 | 0.0032 | 0.0 | 525653 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2724 | 709 | 709 | 488 | 812 | 0.0032 | 0.0 | 525985 |
| logged-in-reader | mobile | reading | /vi/reading/session/44288e02-953d-4741-9a78-84192a353f02 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2715 | 686 | 701 | 444 | 784 | 0.0000 | 0.0 | 631850 |
| logged-in-reader | mobile | reading | /vi/reading/session/2c6f9b39-1427-4aa8-9695-a8991382297b | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2687 | 658 | 673 | 432 | 764 | 0.0000 | 0.0 | 632086 |
| logged-in-reader | mobile | reading | /vi/reading/session/353c56e8-2aff-4e21-bf2e-d348c13591d0 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2772 | 757 | 757 | 568 | 896 | 0.0000 | 0.0 | 631878 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2841 | 821 | 830 | 496 | 816 | 0.0000 | 0.0 | 631049 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2704 | 685 | 686 | 444 | 764 | 0.0000 | 0.0 | 631195 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2825 | 760 | 813 | 480 | 800 | 0.0000 | 0.0 | 631157 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2747 | 719 | 734 | 448 | 788 | 0.0000 | 0.0 | 632765 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2718 | 692 | 705 | 448 | 792 | 0.0000 | 0.0 | 632909 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2806 | 741 | 795 | 460 | 804 | 0.0000 | 0.0 | 633028 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 13 | 4 | 4 | 12 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 6 | 4 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 13 | 4 | 4 | 12 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 6 | 1 | 0 | 6 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 33 | high | 3 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 33 | high | 3 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 32 | high | 3 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | high | 1 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 55 | critical | 2 | 49 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 30 | high | 2 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 32 | high | 0 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/66977a6c-16e2-42b4-9ce0-b7af67b77dff | 33 | high | 2 | 29 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/3ed2c65a-3f97-459c-b081-89ac900bd3ad | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/406f0096-c2fa-4796-a9f1-a2510cc0e200 | 35 | high | 3 | 30 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 31 | high | 2 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 31 | high | 3 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 55 | critical | 2 | 49 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/aa6ce66a-4820-4513-b383-a071f3d5b7e2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/c48ac3bf-3318-46d9-ad10-d83375060178 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/9386ba2f-af68-44b8-a7d6-689e2ff5c809 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 33 | high | 3 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 32 | high | 3 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 32 | high | 2 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 32 | high | 3 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 30 | high | 2 | 26 | 0 |
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
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/0d0d4f30-4b83-4ad4-9fe0-4b955a106f7b | 34 | high | 3 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/0047a5d6-a6e2-46de-8509-9d535afe7580 | 34 | high | 3 | 29 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/f20fe7e3-4f3e-4ca7-b7e6-5b4fd40e6ab3 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 33 | high | 3 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 33 | high | 3 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 36 | critical | 2 | 32 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 32 | high | 3 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 55 | critical | 2 | 49 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 31 | high | 3 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/44288e02-953d-4741-9a78-84192a353f02 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/2c6f9b39-1427-4aa8-9695-a8991382297b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/353c56e8-2aff-4e21-bf2e-d348c13591d0 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 1366 | 310 | html | https://www.tarotnow.xyz/vi/profile |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1340 | 596 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 1237 | 772 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 1171 | 311 | html | https://www.tarotnow.xyz/vi/reading |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1072 | 572 | static | https://www.tarotnow.xyz/_next/static/chunks/0tl7_1-s~69.7.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 1009 | 515 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-out | mobile | auth-public | /vi | GET | 200 | 978 | 555 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 855 | 598 | static | https://www.tarotnow.xyz/_next/static/chunks/172o3d9qvywlw.js |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 840 | 366 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 838 | 335 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 837 | 397 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 835 | 316 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 834 | 323 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 832 | 628 | static | https://www.tarotnow.xyz/_next/static/chunks/04nria0ddfo_c.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 831 | 455 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 829 | 324 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 827 | 397 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 827 | 382 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 826 | 397 | static | https://www.tarotnow.xyz/_next/static/chunks/0tl7_1-s~69.7.js |
| logged-in-admin | desktop | admin | /vi/admin/deposits | GET | 200 | 823 | 340 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 820 | 331 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 820 | 325 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 816 | 343 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 816 | 334 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 814 | 326 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 814 | 322 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 813 | 429 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 812 | 345 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 812 | 338 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 811 | 318 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 809 | 317 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 807 | 285 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 807 | 320 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 806 | 318 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 802 | 304 | html | https://www.tarotnow.xyz/vi/readers |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-out | desktop | auth-public | /vi | GET | 200 | 800 | 555 | static | https://www.tarotnow.xyz/_next/static/chunks/0s_trpk6aw84g.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 800 | 320 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 794 | 325 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 793 | 346 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 793 | 639 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 791 | 334 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 791 | 401 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 790 | 693 | static | https://www.tarotnow.xyz/_next/static/chunks/0qw8t1w~-.eb1.js |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 787 | 315 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 786 | 314 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 786 | 315 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 785 | 310 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 785 | 305 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 785 | 306 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 783 | 103 | static | https://www.tarotnow.xyz/_next/static/media/caa3a2e1cccd8315-s.p.16t1db8_9y2o~.woff2 |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 780 | 541 | static | https://www.tarotnow.xyz/_next/static/chunks/0a0qiftkthquj.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 780 | 310 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 780 | 314 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 780 | 398 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 779 | 317 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 779 | 305 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 776 | 315 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 776 | 300 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 771 | 337 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 770 | 304 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 769 | 397 | static | https://www.tarotnow.xyz/_next/static/chunks/0s_trpk6aw84g.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 765 | 387 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 763 | 303 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | admin | /vi/admin/users | GET | 200 | 761 | 331 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 760 | 338 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 755 | 381 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 753 | 318 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 749 | 352 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 740 | 324 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 737 | 355 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 735 | 334 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 730 | 322 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 728 | 629 | static | https://www.tarotnow.xyz/_next/static/chunks/0i.l9589uvx0j.js |
| logged-in-admin | desktop | auth-public | /vi | GET | 200 | 727 | 462 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 724 | 324 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 723 | 693 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 722 | 347 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | desktop | reader-chat | /vi/chat | GET | 200 | 722 | 318 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 720 | 378 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 720 | 338 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 719 | 337 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 717 | 335 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 716 | 339 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 716 | 708 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | desktop | admin | /vi/admin/gamification | GET | 200 | 715 | 329 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 715 | 304 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 710 | 338 | html | https://www.tarotnow.xyz/vi/admin |
| logged-in-admin | desktop | reading | /vi/reading/session/66977a6c-16e2-42b4-9ce0-b7af67b77dff | GET | 200 | 709 | 414 | html | https://www.tarotnow.xyz/vi/reading/session/66977a6c-16e2-42b4-9ce0-b7af67b77dff |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 708 | 366 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 707 | 398 | static | https://www.tarotnow.xyz/_next/static/chunks/0d2b1s5.nlpim.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 707 | 346 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 706 | 332 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 704 | 398 | static | https://www.tarotnow.xyz/_next/static/chunks/0xovby2vf96zv.js |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | GET | 200 | 703 | 328 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 702 | 318 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | reading | /vi/reading/session/406f0096-c2fa-4796-a9f1-a2510cc0e200 | GET | 200 | 701 | 331 | html | https://www.tarotnow.xyz/vi/reading/session/406f0096-c2fa-4796-a9f1-a2510cc0e200 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 701 | 362 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 700 | 322 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 700 | 360 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | reading | /vi/reading/session/0047a5d6-a6e2-46de-8509-9d535afe7580 | GET | 200 | 699 | 303 | html | https://www.tarotnow.xyz/vi/reading/session/0047a5d6-a6e2-46de-8509-9d535afe7580 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 696 | 318 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | GET | 200 | 693 | 321 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 691 | 329 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 691 | 323 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 689 | 348 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 686 | 325 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 686 | 357 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | GET | 200 | 685 | 329 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 685 | 316 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | GET | 200 | 683 | 327 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | GET | 200 | 680 | 328 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | reading | /vi/reading/session/353c56e8-2aff-4e21-bf2e-d348c13591d0 | GET | 200 | 680 | 330 | html | https://www.tarotnow.xyz/vi/reading/session/353c56e8-2aff-4e21-bf2e-d348c13591d0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 678 | 313 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 677 | 323 | html | https://www.tarotnow.xyz/vi/community |
| logged-out | desktop | auth-public | /vi/reset-password | GET | 200 | 675 | 404 | html | https://www.tarotnow.xyz/vi/reset-password |

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
| logged-in-admin | desktop | reading.init.daily_1: blocked (400). |
| logged-in-admin | desktop | reading.init.spread_3: created 66977a6c-16e2-42b4-9ce0-b7af67b77dff. |
| logged-in-admin | desktop | reading.init.spread_5: created 3ed2c65a-3f97-459c-b081-89ac900bd3ad. |
| logged-in-admin | desktop | reading.init.spread_10: created 406f0096-c2fa-4796-a9f1-a2510cc0e200. |
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
| logged-in-reader | desktop | reading.init.spread_3: created aa6ce66a-4820-4513-b383-a071f3d5b7e2. |
| logged-in-reader | desktop | reading.init.spread_5: created c48ac3bf-3318-46d9-ad10-d83375060178. |
| logged-in-reader | desktop | reading.init.spread_10: created 9386ba2f-af68-44b8-a7d6-689e2ff5c809. |
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
| logged-in-admin | mobile | reading.init.spread_3: created 0d0d4f30-4b83-4ad4-9fe0-4b955a106f7b. |
| logged-in-admin | mobile | reading.init.spread_5: created 0047a5d6-a6e2-46de-8509-9d535afe7580. |
| logged-in-admin | mobile | reading.init.spread_10: created f20fe7e3-4f3e-4ca7-b7e6-5b4fd40e6ab3. |
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
| logged-in-reader | mobile | reading.init.spread_3: created 44288e02-953d-4741-9a78-84192a353f02. |
| logged-in-reader | mobile | reading.init.spread_5: created 2c6f9b39-1427-4aa8-9695-a8991382297b. |
| logged-in-reader | mobile | reading.init.spread_10: created 353c56e8-2aff-4e21-bf2e-d348c13591d0. |
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
