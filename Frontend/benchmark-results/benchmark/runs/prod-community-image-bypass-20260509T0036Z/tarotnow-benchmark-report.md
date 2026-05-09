# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-09T00:45:40.487Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 0
- High pages (request count): 142
- High slow requests: 33
- Medium slow requests: 277

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 2874 | 224 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 3024 | 1240 | 0 | 0 | 13 | 0 | yes |
| logged-in-reader | desktop | 33 | 3034 | 948 | 0 | 0 | 7 | 0 | yes |
| logged-out | mobile | 9 | 2766 | 225 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 2944 | 1237 | 0 | 0 | 8 | 0 | yes |
| logged-in-reader | mobile | 33 | 2954 | 948 | 0 | 0 | 12 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.5 | 2935 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2866 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 7284 | 0 |
| logged-in-admin | desktop | community | 1 | 35.0 | 4128 | 0 |
| logged-in-admin | desktop | gacha | 2 | 32.0 | 2964 | 0 |
| logged-in-admin | desktop | gamification | 1 | 30.0 | 3263 | 0 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2719 | 0 |
| logged-in-admin | desktop | inventory | 1 | 30.0 | 2852 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 31.0 | 3251 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2803 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2840 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.0 | 2865 | 0 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 3196 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.1 | 2812 | 0 |
| logged-in-admin | desktop | reading | 5 | 29.0 | 2889 | 0 |
| logged-in-admin | desktop | wallet | 4 | 28.0 | 2808 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.2 | 2842 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 2962 | 0 |
| logged-in-admin | mobile | collection | 1 | 29.0 | 5910 | 0 |
| logged-in-admin | mobile | community | 1 | 35.0 | 3751 | 0 |
| logged-in-admin | mobile | gacha | 2 | 33.0 | 2872 | 0 |
| logged-in-admin | mobile | gamification | 1 | 29.0 | 2864 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 2771 | 0 |
| logged-in-admin | mobile | inventory | 1 | 31.0 | 2886 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 2804 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 2736 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 2724 | 0 |
| logged-in-admin | mobile | profile | 3 | 28.7 | 2926 | 0 |
| logged-in-admin | mobile | reader | 1 | 28.0 | 2930 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 2859 | 0 |
| logged-in-admin | mobile | reading | 5 | 28.6 | 2855 | 0 |
| logged-in-admin | mobile | wallet | 4 | 28.3 | 2892 | 0 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 2722 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6969 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3649 | 0 |
| logged-in-reader | desktop | gacha | 2 | 30.0 | 2864 | 0 |
| logged-in-reader | desktop | gamification | 1 | 30.0 | 2922 | 0 |
| logged-in-reader | desktop | home | 1 | 26.0 | 2772 | 0 |
| logged-in-reader | desktop | inventory | 1 | 35.0 | 3024 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 30.0 | 2904 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2751 | 0 |
| logged-in-reader | desktop | notifications | 1 | 28.0 | 2871 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.7 | 3051 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 3029 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.3 | 2814 | 0 |
| logged-in-reader | desktop | reading | 5 | 30.0 | 2944 | 0 |
| logged-in-reader | desktop | wallet | 4 | 28.0 | 2929 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2823 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5688 | 0 |
| logged-in-reader | mobile | community | 1 | 35.0 | 3783 | 0 |
| logged-in-reader | mobile | gacha | 2 | 31.0 | 2884 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2865 | 0 |
| logged-in-reader | mobile | home | 1 | 26.0 | 2776 | 0 |
| logged-in-reader | mobile | inventory | 1 | 31.0 | 2876 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 30.0 | 2688 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2726 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2891 | 0 |
| logged-in-reader | mobile | profile | 3 | 29.7 | 2946 | 0 |
| logged-in-reader | mobile | reader | 1 | 29.0 | 2940 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.1 | 2829 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.4 | 2841 | 0 |
| logged-in-reader | mobile | wallet | 4 | 28.3 | 2843 | 0 |
| logged-out | desktop | auth | 5 | 24.0 | 2776 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 3803 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2729 | 0 |
| logged-out | mobile | auth | 5 | 24.0 | 2713 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3037 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 2764 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3803 | 1332 | 1780 | 1324 | 1324 | 0.0000 | 651.0 | 600862 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2849 | 747 | 826 | 596 | 596 | 0.0000 | 0.0 | 511706 |
| logged-out | desktop | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2869 | 821 | 853 | 700 | 700 | 0.0000 | 0.0 | 512303 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2739 | 731 | 731 | 536 | 536 | 0.0000 | 0.0 | 511300 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2685 | 676 | 677 | 488 | 488 | 0.0000 | 0.0 | 511562 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2737 | 731 | 731 | 532 | 532 | 0.0000 | 0.0 | 511450 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2735 | 725 | 725 | 500 | 832 | 0.0000 | 0.0 | 525276 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 739 | 739 | 504 | 832 | 0.0000 | 0.0 | 525321 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2707 | 698 | 698 | 496 | 824 | 0.0000 | 0.0 | 525360 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2719 | 681 | 713 | 680 | 1420 | 0.0039 | 369.0 | 537061 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2930 | 889 | 923 | 500 | 992 | 0.0041 | 0.0 | 642302 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 30 | 9 | high | 0 | 0 | 0 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2852 | 833 | 838 | 520 | 1204 | 0.0041 | 0.0 | 644205 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2931 | 917 | 917 | 580 | 1256 | 0.0041 | 0.0 | 726201 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2996 | 971 | 988 | 560 | 1376 | 0.0041 | 0.0 | 727384 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 26 | high | 0 | 0 | 0 | 0 | 13 | 3 | 5 | 12 | 1 | 0 | 7284 | 799 | 821 | 588 | 588 | 0.0042 | 169.0 | 642758 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3000 | 813 | 991 | 616 | 1012 | 0.0489 | 0.0 | 635425 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2808 | 772 | 801 | 632 | 1020 | 0.0041 | 0.0 | 631199 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 38 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2788 | 856 | 982 | 888 | 1224 | 0.0489 | 0.0 | 630429 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2885 | 866 | 866 | 524 | 1212 | 0.0041 | 0.0 | 634377 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2866 | 837 | 855 | 540 | 1020 | 0.0041 | 0.0 | 631310 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3251 | 814 | 1243 | 736 | 1148 | 0.0041 | 0.0 | 651124 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | 5 | high | 0 | 0 | 1 | 0 | 2 | 0 | 0 | 2 | 0 | 0 | 4128 | 848 | 1348 | 596 | 1380 | 0.0041 | 0.0 | 794709 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3263 | 891 | 1253 | 528 | 968 | 0.0279 | 0.0 | 643160 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2874 | 853 | 859 | 568 | 1356 | 0.0041 | 0.0 | 633465 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2758 | 734 | 749 | 528 | 992 | 0.0041 | 0.0 | 630968 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2822 | 793 | 812 | 608 | 1328 | 0.0041 | 0.0 | 631912 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 36 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2778 | 859 | 860 | 524 | 1268 | 0.0041 | 0.0 | 630605 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2840 | 768 | 830 | 556 | 988 | 0.0046 | 0.0 | 631627 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3196 | 1175 | 1189 | 1076 | 1492 | 0.0041 | 0.0 | 631964 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2758 | 746 | 746 | 556 | 1320 | 0.0041 | 0.0 | 632243 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2849 | 826 | 840 | 600 | 908 | 0.0020 | 0.0 | 525631 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2700 | 658 | 689 | 540 | 872 | 0.0020 | 0.0 | 525696 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2861 | 769 | 853 | 676 | 996 | 0.0020 | 0.0 | 525823 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3228 | 720 | 1193 | 680 | 1116 | 0.0000 | 0.0 | 647007 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2736 | 689 | 722 | 552 | 1156 | 0.0000 | 0.0 | 647106 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2821 | 674 | 812 | 632 | 976 | 0.0000 | 0.0 | 645376 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3291 | 803 | 1279 | 696 | 1112 | 0.0022 | 0.0 | 698938 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2888 | 854 | 881 | 584 | 936 | 0.0000 | 0.0 | 644181 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2894 | 860 | 885 | 568 | 972 | 0.0000 | 0.0 | 646120 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2903 | 739 | 789 | 608 | 1208 | 0.0000 | 0.0 | 648231 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3012 | 915 | 1001 | 588 | 1200 | 0.0000 | 0.0 | 688791 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2764 | 751 | 753 | 520 | 944 | 0.0000 | 0.0 | 649432 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2811 | 786 | 804 | 564 | 936 | 0.0000 | 0.0 | 645587 |
| logged-in-admin | desktop | reading | /vi/reading/session/97d1c31e-faf4-4e03-b025-1cb780806f8a | 32 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2943 | 784 | 929 | 544 | 976 | 0.0054 | 0.0 | 713143 |
| logged-in-admin | desktop | reading | /vi/reading/session/29bf38b4-07f7-44b7-9de9-51487ad4e9ea | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3069 | 1058 | 1058 | 912 | 1284 | 0.0041 | 0.0 | 631909 |
| logged-in-admin | desktop | reading | /vi/reading/session/2f301395-d111-4dff-98d5-349169638fd3 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2745 | 721 | 735 | 648 | 1036 | 0.0041 | 0.0 | 631908 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2813 | 782 | 805 | 552 | 960 | 0.0041 | 0.0 | 630825 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2822 | 776 | 814 | 568 | 984 | 0.0041 | 0.0 | 630891 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2914 | 850 | 892 | 516 | 984 | 0.0041 | 0.0 | 630547 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2699 | 675 | 688 | 484 | 1132 | 0.0041 | 0.0 | 632296 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2735 | 716 | 723 | 536 | 1236 | 0.0041 | 0.0 | 632365 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2815 | 790 | 804 | 720 | 1440 | 0.0041 | 0.0 | 632468 |
| logged-in-reader | desktop | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2772 | 684 | 763 | 532 | 1260 | 0.0033 | 403.0 | 536997 |
| logged-in-reader | desktop | reading | /vi/reading | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2920 | 870 | 882 | 588 | 976 | 0.0037 | 0.0 | 641337 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 35 | 4 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 3024 | 989 | 1002 | 560 | 1152 | 0.0037 | 1.0 | 651796 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 30 | 10 | high | 0 | 0 | 0 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2861 | 842 | 842 | 512 | 1196 | 0.0037 | 0.0 | 724586 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 855 | 860 | 596 | 1392 | 0.0037 | 0.0 | 723703 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 24 | high | 0 | 0 | 0 | 0 | 6 | 4 | 0 | 6 | 0 | 0 | 6969 | 794 | 854 | 504 | 992 | 0.0037 | 0.0 | 641693 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3052 | 863 | 1040 | 608 | 1104 | 0.0723 | 0.0 | 637207 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3129 | 1045 | 1065 | 1192 | 1192 | 0.0037 | 0.0 | 631321 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2973 | 931 | 944 | 564 | 1400 | 0.0037 | 0.0 | 631943 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 30 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2907 | 896 | 896 | 508 | 1168 | 0.0037 | 0.0 | 635276 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2722 | 697 | 711 | 536 | 988 | 0.0037 | 0.0 | 631130 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2904 | 849 | 881 | 564 | 1044 | 0.0037 | 0.0 | 650239 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | 9 | high | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3649 | 889 | 910 | 812 | 1344 | 0.0037 | 0.0 | 642509 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 30 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2922 | 907 | 907 | 536 | 1056 | 0.0274 | 0.0 | 643327 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3055 | 1043 | 1043 | 604 | 1428 | 0.0037 | 0.0 | 633675 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2994 | 830 | 909 | 564 | 1088 | 0.0037 | 0.0 | 631254 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2794 | 773 | 781 | 700 | 1108 | 0.0037 | 0.0 | 631126 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2872 | 851 | 859 | 648 | 1112 | 0.0092 | 0.0 | 632606 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2871 | 843 | 843 | 520 | 1228 | 0.0037 | 0.0 | 631857 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3029 | 945 | 1018 | 584 | 1076 | 0.0037 | 0.0 | 632044 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2902 | 858 | 871 | 564 | 1280 | 0.0037 | 0.0 | 632253 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2794 | 783 | 783 | 668 | 944 | 0.0018 | 0.0 | 525678 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2728 | 716 | 716 | 560 | 876 | 0.0018 | 0.0 | 525590 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2730 | 717 | 717 | 520 | 836 | 0.0018 | 0.0 | 525817 |
| logged-in-reader | desktop | reading | /vi/reading/session/6cb71599-47fa-459b-91b7-14194d917fdf | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3123 | 941 | 1109 | 680 | 1112 | 0.0037 | 0.0 | 724662 |
| logged-in-reader | desktop | reading | /vi/reading/session/474df09a-4a03-4cbc-8106-9c3b11fbe1f0 | 32 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2907 | 815 | 896 | 540 | 908 | 0.0037 | 0.0 | 713374 |
| logged-in-reader | desktop | reading | /vi/reading/session/91aeb338-ac82-440d-af06-1be35041cea7 | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2867 | 826 | 857 | 612 | 1072 | 0.0049 | 0.0 | 631980 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2751 | 655 | 677 | 524 | 920 | 0.0037 | 0.0 | 630723 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2857 | 760 | 833 | 576 | 924 | 0.0037 | 0.0 | 630661 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2774 | 741 | 762 | 520 | 932 | 0.0037 | 0.0 | 630795 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2746 | 718 | 733 | 544 | 1252 | 0.0037 | 0.0 | 632213 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2779 | 758 | 770 | 528 | 1212 | 0.0037 | 0.0 | 632671 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2881 | 859 | 860 | 576 | 1204 | 0.0037 | 0.0 | 632846 |
| logged-out | mobile | auth-public | /vi | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3037 | 950 | 1027 | 596 | 960 | 0.0000 | 0.0 | 602123 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2699 | 674 | 690 | 636 | 636 | 0.0000 | 0.0 | 511712 |
| logged-out | mobile | auth-public | /vi/register | 24 | 2 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2728 | 716 | 716 | 484 | 484 | 0.0000 | 0.0 | 512287 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2743 | 730 | 730 | 484 | 484 | 0.0000 | 0.0 | 511290 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2647 | 635 | 635 | 456 | 456 | 0.0000 | 0.0 | 511488 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2749 | 740 | 740 | 468 | 468 | 0.0000 | 0.0 | 511532 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 712 | 712 | 448 | 820 | 0.0000 | 0.0 | 525326 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2705 | 692 | 692 | 456 | 456 | 0.0000 | 0.0 | 525264 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2819 | 811 | 811 | 472 | 472 | 0.0000 | 0.0 | 525425 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2771 | 761 | 761 | 580 | 948 | 0.0032 | 0.0 | 537111 |
| logged-in-admin | mobile | reading | /vi/reading | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2871 | 854 | 859 | 624 | 984 | 0.0000 | 0.0 | 641640 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 31 | 8 | high | 0 | 0 | 1 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 2886 | 864 | 864 | 460 | 1096 | 0.0000 | 0.0 | 644763 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 35 | 6 | high | 0 | 0 | 0 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 2858 | 836 | 836 | 488 | 848 | 0.0000 | 0.0 | 796544 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2886 | 867 | 867 | 452 | 872 | 0.0000 | 0.0 | 726413 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | 26 | high | 0 | 0 | 0 | 0 | 13 | 3 | 5 | 12 | 1 | 0 | 5910 | 723 | 727 | 452 | 452 | 0.0000 | 16.0 | 642867 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3096 | 815 | 1080 | 456 | 852 | 0.0000 | 0.0 | 634653 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 837 | 849 | 496 | 840 | 0.0000 | 0.0 | 631098 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2818 | 882 | 1021 | 644 | 1004 | 0.0760 | 0.0 | 630262 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2910 | 898 | 898 | 464 | 1096 | 0.0000 | 0.0 | 634081 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2962 | 946 | 947 | 672 | 1000 | 0.0000 | 0.0 | 631038 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2804 | 777 | 788 | 556 | 932 | 0.0000 | 0.0 | 650053 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 35 | 3 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3751 | 862 | 1007 | 516 | 1036 | 0.0000 | 0.0 | 794315 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2864 | 846 | 846 | 464 | 848 | 0.0000 | 0.0 | 641950 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2932 | 921 | 921 | 492 | 1168 | 0.0000 | 0.0 | 634205 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2821 | 806 | 807 | 544 | 888 | 0.0000 | 0.0 | 631077 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2938 | 883 | 926 | 508 | 892 | 0.0000 | 0.0 | 631920 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 37 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2875 | 859 | 864 | 696 | 1120 | 0.0071 | 0.0 | 630546 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2724 | 709 | 710 | 480 | 832 | 0.0000 | 0.0 | 631638 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2930 | 905 | 916 | 480 | 884 | 0.0000 | 0.0 | 631898 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2722 | 710 | 710 | 448 | 808 | 0.0000 | 0.0 | 632523 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2800 | 788 | 788 | 448 | 792 | 0.0032 | 0.0 | 525651 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2714 | 702 | 702 | 508 | 840 | 0.0032 | 0.0 | 525752 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2694 | 675 | 675 | 456 | 792 | 0.0032 | 0.0 | 525885 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2772 | 754 | 756 | 500 | 832 | 0.0000 | 0.0 | 647087 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2834 | 811 | 818 | 488 | 884 | 0.0000 | 0.0 | 647163 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2845 | 814 | 830 | 556 | 864 | 0.0000 | 0.0 | 645391 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2872 | 850 | 859 | 652 | 980 | 0.0000 | 0.0 | 664447 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2778 | 726 | 755 | 476 | 812 | 0.0000 | 0.0 | 644115 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2736 | 703 | 717 | 456 | 784 | 0.0000 | 0.0 | 645905 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2903 | 879 | 879 | 520 | 892 | 0.0000 | 0.0 | 648116 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2930 | 822 | 915 | 504 | 852 | 0.0000 | 0.0 | 688909 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2865 | 845 | 848 | 512 | 1156 | 0.0000 | 0.0 | 649486 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2884 | 864 | 867 | 480 | 840 | 0.0000 | 0.0 | 645536 |
| logged-in-admin | mobile | reading | /vi/reading/session/b098803d-a599-4222-b462-44ecb8eaffe7 | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2909 | 870 | 894 | 736 | 1076 | 0.0000 | 0.0 | 631860 |
| logged-in-admin | mobile | reading | /vi/reading/session/c46e3f73-436d-4b30-8885-ab45de051571 | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2890 | 863 | 876 | 464 | 816 | 0.0000 | 0.0 | 681107 |
| logged-in-admin | mobile | reading | /vi/reading/session/efe733dd-c6da-4d12-86b4-eb915ab46895 | 28 | 8 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2881 | 868 | 869 | 588 | 940 | 0.0000 | 0.0 | 631782 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 738 | 741 | 452 | 780 | 0.0000 | 0.0 | 630689 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2784 | 768 | 773 | 492 | 812 | 0.0000 | 0.0 | 630736 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2877 | 852 | 854 | 460 | 812 | 0.0000 | 0.0 | 631374 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2823 | 800 | 810 | 448 | 812 | 0.0000 | 0.0 | 632200 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2955 | 933 | 944 | 548 | 916 | 0.0000 | 0.0 | 632287 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2909 | 819 | 841 | 628 | 996 | 0.0000 | 0.0 | 632636 |
| logged-in-reader | mobile | auth-public | /vi | 26 | 12 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2776 | 760 | 760 | 496 | 916 | 0.0028 | 0.0 | 537097 |
| logged-in-reader | mobile | reading | /vi/reading | 30 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2880 | 834 | 867 | 464 | 836 | 0.0000 | 0.0 | 642981 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 31 | 8 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2876 | 865 | 865 | 484 | 1120 | 0.0000 | 0.0 | 644782 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 32 | 9 | high | 0 | 0 | 2 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2877 | 862 | 862 | 448 | 1104 | 0.0000 | 0.0 | 726294 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2890 | 875 | 875 | 464 | 1152 | 0.0000 | 0.0 | 723533 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 23 | high | 0 | 0 | 0 | 0 | 5 | 3 | 1 | 5 | 0 | 0 | 5688 | 724 | 744 | 476 | 476 | 0.0000 | 0.0 | 641665 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3098 | 822 | 1068 | 440 | 1080 | 0.0821 | 0.0 | 637212 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2899 | 831 | 879 | 480 | 872 | 0.0000 | 0.0 | 631707 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2842 | 821 | 832 | 484 | 828 | 0.0000 | 0.0 | 631765 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2908 | 863 | 889 | 512 | 900 | 0.0000 | 0.0 | 633428 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2823 | 787 | 805 | 524 | 868 | 0.0000 | 0.0 | 631176 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2688 | 652 | 671 | 436 | 800 | 0.0000 | 0.0 | 649850 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 35 | 3 | high | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 3783 | 889 | 1032 | 552 | 1336 | 0.0051 | 0.0 | 794466 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2865 | 846 | 846 | 452 | 824 | 0.0000 | 0.0 | 642193 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2875 | 853 | 854 | 452 | 1160 | 0.0000 | 0.0 | 634354 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2735 | 697 | 718 | 496 | 848 | 0.0000 | 0.0 | 631002 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2873 | 796 | 859 | 516 | 848 | 0.0000 | 0.0 | 631398 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2889 | 869 | 869 | 528 | 1176 | 0.0330 | 0.0 | 632688 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2891 | 871 | 871 | 528 | 884 | 0.0000 | 0.0 | 631824 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 29 | 4 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2940 | 880 | 927 | 472 | 912 | 0.0000 | 0.0 | 632516 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2853 | 795 | 842 | 476 | 836 | 0.0000 | 0.0 | 632507 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2682 | 657 | 657 | 476 | 792 | 0.0028 | 0.0 | 525619 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2689 | 672 | 672 | 480 | 816 | 0.0028 | 0.0 | 525627 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2807 | 792 | 792 | 528 | 860 | 0.0028 | 0.0 | 525653 |
| logged-in-reader | mobile | reading | /vi/reading/session/7aec6104-c5de-46a4-829c-04d21e06aa4d | 33 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2977 | 756 | 961 | 480 | 836 | 0.0000 | 0.0 | 693073 |
| logged-in-reader | mobile | reading | /vi/reading/session/0a2d8e46-108f-4f9d-aa9f-2581183807ff | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2788 | 754 | 772 | 500 | 840 | 0.0000 | 0.0 | 631920 |
| logged-in-reader | mobile | reading | /vi/reading/session/7ab89974-9ab4-4b27-ae8a-93965f8bfd71 | 28 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2709 | 670 | 691 | 444 | 784 | 0.0000 | 0.0 | 631812 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2752 | 721 | 736 | 568 | 892 | 0.0000 | 0.0 | 630753 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2767 | 731 | 751 | 556 | 892 | 0.0000 | 0.0 | 630837 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2891 | 828 | 866 | 476 | 852 | 0.0000 | 0.0 | 631419 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2820 | 760 | 804 | 456 | 808 | 0.0000 | 0.0 | 632355 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2821 | 744 | 807 | 452 | 816 | 0.0000 | 0.0 | 632599 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2845 | 817 | 827 | 484 | 832 | 0.0000 | 0.0 | 632827 |

## Collection Image Focus
| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | /vi/collection | 13 | 3 | 5 | 12 | 1 | 0 |
| logged-in-reader | desktop | /vi/collection | 6 | 4 | 0 | 6 | 0 | 0 |
| logged-in-admin | mobile | /vi/collection | 13 | 3 | 5 | 12 | 1 | 0 |
| logged-in-reader | mobile | /vi/collection | 5 | 3 | 1 | 5 | 0 | 0 |

## Suspicious Pages (>25 requests)
| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |
| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 32 | high | 2 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 35 | high | 1 | 31 | 1 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 33 | high | 1 | 30 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/97d1c31e-faf4-4e03-b025-1cb780806f8a | 32 | high | 1 | 29 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/29bf38b4-07f7-44b7-9de9-51487ad4e9ea | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/2f301395-d111-4dff-98d5-349169638fd3 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 35 | high | 0 | 33 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/6cb71599-47fa-459b-91b7-14194d917fdf | 34 | high | 2 | 30 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/474df09a-4a03-4cbc-8106-9c3b11fbe1f0 | 32 | high | 1 | 29 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/91aeb338-ac82-440d-af06-1be35041cea7 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 35 | high | 0 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 35 | high | 1 | 31 | 1 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/b098803d-a599-4222-b462-44ecb8eaffe7 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/c46e3f73-436d-4b30-8885-ab45de051571 | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/efe733dd-c6da-4d12-86b4-eb915ab46895 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 30 | high | 2 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 32 | high | 2 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 32 | high | 2 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 35 | high | 1 | 31 | 1 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/7aec6104-c5de-46a4-829c-04d21e06aa4d | 33 | high | 2 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/0a2d8e46-108f-4f9d-aa9f-2581183807ff | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/7ab89974-9ab4-4b27-ae8a-93965f8bfd71 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 1095 | 343 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1046 | 538 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8hzxtf1-_dk.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1043 | 568 | static | https://www.tarotnow.xyz/_next/static/chunks/12oo753jmxg~2.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1030 | 357 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 955 | 392 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-admin | desktop | reading | /vi/reading/session/29bf38b4-07f7-44b7-9de9-51487ad4e9ea | GET | 200 | 954 | 323 | html | https://www.tarotnow.xyz/vi/reading/session/29bf38b4-07f7-44b7-9de9-51487ad4e9ea |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 950 | 331 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 932 | 681 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Frare_title_lucky_star_50_20260416_165453.avif&w=256&q=75 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 913 | 322 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 909 | 325 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | GET | 200 | 893 | 318 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 882 | 542 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-reader | desktop | reading | /vi/reading/session/6cb71599-47fa-459b-91b7-14194d917fdf | GET | 200 | 866 | 319 | html | https://www.tarotnow.xyz/vi/reading/session/6cb71599-47fa-459b-91b7-14194d917fdf |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 851 | 317 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 843 | 354 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 838 | 319 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 838 | 319 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 833 | 326 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 832 | 324 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 829 | 333 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | GET | 200 | 822 | 323 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | GET | 200 | 820 | 356 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 820 | 355 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 818 | 327 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 818 | 339 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 814 | 327 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 813 | 377 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | GET | 200 | 812 | 313 | html | https://www.tarotnow.xyz/vi/reader/apply |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 805 | 350 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 805 | 325 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 802 | 319 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 801 | 320 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | reading | /vi/reading/session/efe733dd-c6da-4d12-86b4-eb915ab46895 | GET | 200 | 801 | 317 | html | https://www.tarotnow.xyz/vi/reading/session/efe733dd-c6da-4d12-86b4-eb915ab46895 |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 799 | 335 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 798 | 323 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 798 | 330 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 797 | 317 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 794 | 315 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 793 | 321 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 793 | 318 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 792 | 540 | static | https://www.tarotnow.xyz/_next/static/chunks/0mp2dyp5p7fbx.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 792 | 315 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 791 | 338 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 791 | 304 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 790 | 320 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | GET | 200 | 789 | 315 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | desktop | reading | /vi/reading | GET | 200 | 789 | 316 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 789 | 379 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 786 | 324 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 783 | 326 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 782 | 306 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 781 | 303 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 780 | 312 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | desktop | admin | /vi/admin | GET | 200 | 779 | 520 | static | https://www.tarotnow.xyz/_next/static/chunks/0f8m2dxx~fr_z.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 779 | 307 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 778 | 324 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | reading | /vi/reading | GET | 200 | 777 | 311 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 776 | 319 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 775 | 307 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 774 | 342 | html | https://www.tarotnow.xyz/vi/wallet/deposit |
| logged-in-admin | mobile | reading | /vi/reading/session/b098803d-a599-4222-b462-44ecb8eaffe7 | GET | 200 | 773 | 399 | html | https://www.tarotnow.xyz/vi/reading/session/b098803d-a599-4222-b462-44ecb8eaffe7 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 772 | 317 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 772 | 335 | html | https://www.tarotnow.xyz/vi/admin/users |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 771 | 316 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 771 | 313 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 770 | 312 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 770 | 334 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 770 | 312 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 768 | 317 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-out | mobile | auth-public | /vi | GET | 200 | 768 | 334 | html | https://www.tarotnow.xyz/vi |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 768 | 312 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 764 | 332 | html | https://www.tarotnow.xyz/vi/leaderboard |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 762 | 316 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 761 | 374 | html | https://www.tarotnow.xyz/vi/community |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 760 | 311 | html | https://www.tarotnow.xyz/vi/readers/69e7b9fa5b238c89fadb3a0b |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 756 | 323 | html | https://www.tarotnow.xyz/vi/admin/disputes |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 755 | 327 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-reader | desktop | reading | /vi/reading/session/91aeb338-ac82-440d-af06-1be35041cea7 | GET | 200 | 754 | 373 | html | https://www.tarotnow.xyz/vi/reading/session/91aeb338-ac82-440d-af06-1be35041cea7 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 753 | 320 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 752 | 416 | static | https://www.tarotnow.xyz/_next/static/chunks/0b8hzxtf1-_dk.js |
| logged-in-reader | desktop | reading | /vi/reading/history | GET | 200 | 751 | 324 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 748 | 333 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | GET | 200 | 747 | 377 | html | https://www.tarotnow.xyz/vi/admin/reader-requests |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 746 | 352 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 745 | 327 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 744 | 733 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon%2Fpower_booster_50_20260416_165453.avif&w=256&q=75 |
| logged-in-reader | mobile | reader-chat | /vi/chat | GET | 200 | 740 | 404 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 739 | 328 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | GET | 200 | 739 | 330 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 739 | 316 | html | https://www.tarotnow.xyz/vi/collection |
| logged-in-reader | mobile | reading | /vi/reading/history | GET | 200 | 738 | 314 | html | https://www.tarotnow.xyz/vi/reading/history |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 738 | 327 | html | https://www.tarotnow.xyz/vi/legal/ai-disclaimer |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 736 | 380 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | desktop | reading | /vi/reading/session/97d1c31e-faf4-4e03-b025-1cb780806f8a | GET | 200 | 734 | 317 | html | https://www.tarotnow.xyz/vi/reading/session/97d1c31e-faf4-4e03-b025-1cb780806f8a |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | GET | 200 | 731 | 318 | html | https://www.tarotnow.xyz/vi/legal/tos |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 725 | 385 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | desktop | admin | /vi/admin/gamification | GET | 200 | 725 | 397 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 725 | 330 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 724 | 437 | static | https://www.tarotnow.xyz/_next/static/chunks/12oo753jmxg~2.js |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 724 | 323 | html | https://www.tarotnow.xyz/vi/readers/97410d49-ab32-469d-b421-f57321b262a2 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 723 | 417 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | GET | 200 | 721 | 417 | static | https://www.tarotnow.xyz/_next/static/chunks/0zfpu6y4~e9el.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 719 | 336 | html | https://www.tarotnow.xyz/vi/profile/reader |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 719 | 314 | html | https://www.tarotnow.xyz/vi/readers/69d93c24bc68b27090414f6c |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 717 | 315 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 717 | 376 | html | https://www.tarotnow.xyz/vi/readers/69dbe86b052d3c8f3f55e231 |
| logged-in-reader | desktop | reading | /vi/reading/session/474df09a-4a03-4cbc-8106-9c3b11fbe1f0 | GET | 200 | 717 | 322 | html | https://www.tarotnow.xyz/vi/reading/session/474df09a-4a03-4cbc-8106-9c3b11fbe1f0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 714 | 330 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 714 | 324 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | GET | 200 | 714 | 313 | html | https://www.tarotnow.xyz/vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 713 | 324 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 713 | 313 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-reader | mobile | reading | /vi/reading/session/7aec6104-c5de-46a4-829c-04d21e06aa4d | GET | 200 | 712 | 343 | html | https://www.tarotnow.xyz/vi/reading/session/7aec6104-c5de-46a4-829c-04d21e06aa4d |

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
| logged-in-admin | desktop | reading.init.spread_3: created 97d1c31e-faf4-4e03-b025-1cb780806f8a. |
| logged-in-admin | desktop | reading.init.spread_5: created 29bf38b4-07f7-44b7-9de9-51487ad4e9ea. |
| logged-in-admin | desktop | reading.init.spread_10: created 2f301395-d111-4dff-98d5-349169638fd3. |
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
| logged-in-reader | desktop | reading.init.spread_3: created 6cb71599-47fa-459b-91b7-14194d917fdf. |
| logged-in-reader | desktop | reading.init.spread_5: created 474df09a-4a03-4cbc-8106-9c3b11fbe1f0. |
| logged-in-reader | desktop | reading.init.spread_10: created 91aeb338-ac82-440d-af06-1be35041cea7. |
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
| logged-in-admin | mobile | reading.init.spread_3: created b098803d-a599-4222-b462-44ecb8eaffe7. |
| logged-in-admin | mobile | reading.init.spread_5: created c46e3f73-436d-4b30-8885-ab45de051571. |
| logged-in-admin | mobile | reading.init.spread_10: created efe733dd-c6da-4d12-86b4-eb915ab46895. |
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
| logged-in-reader | mobile | reading.init.spread_3: created 7aec6104-c5de-46a4-829c-04d21e06aa4d. |
| logged-in-reader | mobile | reading.init.spread_5: created 0a2d8e46-108f-4f9d-aa9f-2581183807ff. |
| logged-in-reader | mobile | reading.init.spread_10: created 7ab89974-9ab4-4b27-ae8a-93965f8bfd71. |
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
