# TarotNow Navigation Benchmark Report

- Generated at (UTC): 2026-05-08T17:54:33.188Z
- Base URL: https://www.tarotnow.xyz/vi
- Benchmark mode: full-matrix
- Thresholds: >35 requests = Critical, >25 = High, request >800ms = High, >400ms = Medium
- Critical pages (request count): 9
- High pages (request count): 142
- High slow requests: 262
- Medium slow requests: 753

## Scenario Summary
| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 3186 | 225 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | desktop | 43 | 2993 | 1286 | 2 | 0 | 8 | 0 | yes |
| logged-in-reader | desktop | 33 | 3032 | 978 | 0 | 0 | 11 | 0 | yes |
| logged-out | mobile | 9 | 2911 | 226 | 0 | 0 | 0 | 0 | yes |
| logged-in-admin | mobile | 43 | 3777 | 1266 | 6 | 0 | 20 | 0 | yes |
| logged-in-reader | mobile | 33 | 3098 | 985 | 1 | 0 | 11 | 0 | yes |

## Route-Family Summary
| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |
| --- | --- | --- | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | admin | 10 | 29.2 | 2855 | 0 |
| logged-in-admin | desktop | chat | 1 | 28.0 | 2847 | 0 |
| logged-in-admin | desktop | collection | 1 | 29.0 | 6560 | 0 |
| logged-in-admin | desktop | community | 1 | 34.0 | 3774 | 0 |
| logged-in-admin | desktop | gacha | 2 | 33.5 | 2912 | 0 |
| logged-in-admin | desktop | gamification | 1 | 29.0 | 3182 | 1 |
| logged-in-admin | desktop | home | 1 | 26.0 | 2694 | 0 |
| logged-in-admin | desktop | inventory | 1 | 33.0 | 3112 | 0 |
| logged-in-admin | desktop | leaderboard | 1 | 30.0 | 2887 | 0 |
| logged-in-admin | desktop | legal | 3 | 25.0 | 2751 | 0 |
| logged-in-admin | desktop | notifications | 1 | 28.0 | 2896 | 0 |
| logged-in-admin | desktop | profile | 3 | 29.3 | 2908 | 1 |
| logged-in-admin | desktop | reader | 1 | 28.0 | 2753 | 0 |
| logged-in-admin | desktop | readers | 7 | 28.0 | 2797 | 0 |
| logged-in-admin | desktop | reading | 5 | 28.0 | 2804 | 0 |
| logged-in-admin | desktop | wallet | 4 | 40.8 | 3261 | 0 |
| logged-in-admin | mobile | admin | 10 | 29.8 | 3641 | 0 |
| logged-in-admin | mobile | chat | 1 | 28.0 | 3479 | 0 |
| logged-in-admin | mobile | collection | 1 | 30.0 | 6220 | 0 |
| logged-in-admin | mobile | community | 1 | 38.0 | 4866 | 0 |
| logged-in-admin | mobile | gacha | 2 | 32.5 | 3563 | 0 |
| logged-in-admin | mobile | gamification | 1 | 30.0 | 3310 | 0 |
| logged-in-admin | mobile | home | 1 | 26.0 | 3456 | 0 |
| logged-in-admin | mobile | inventory | 1 | 37.0 | 3554 | 0 |
| logged-in-admin | mobile | leaderboard | 1 | 30.0 | 4211 | 0 |
| logged-in-admin | mobile | legal | 3 | 25.0 | 3711 | 0 |
| logged-in-admin | mobile | notifications | 1 | 28.0 | 4940 | 0 |
| logged-in-admin | mobile | profile | 3 | 29.0 | 4123 | 3 |
| logged-in-admin | mobile | reader | 1 | 31.0 | 3291 | 0 |
| logged-in-admin | mobile | readers | 7 | 28.3 | 3846 | 2 |
| logged-in-admin | mobile | reading | 5 | 29.6 | 3605 | 0 |
| logged-in-admin | mobile | wallet | 4 | 29.3 | 3271 | 1 |
| logged-in-reader | desktop | chat | 1 | 28.0 | 3033 | 0 |
| logged-in-reader | desktop | collection | 1 | 29.0 | 6906 | 0 |
| logged-in-reader | desktop | community | 1 | 30.0 | 3479 | 0 |
| logged-in-reader | desktop | gacha | 2 | 33.5 | 2965 | 0 |
| logged-in-reader | desktop | gamification | 1 | 29.0 | 3036 | 0 |
| logged-in-reader | desktop | home | 1 | 29.0 | 3142 | 0 |
| logged-in-reader | desktop | inventory | 1 | 31.0 | 2841 | 0 |
| logged-in-reader | desktop | leaderboard | 1 | 33.0 | 3194 | 0 |
| logged-in-reader | desktop | legal | 3 | 25.0 | 2713 | 0 |
| logged-in-reader | desktop | notifications | 1 | 29.0 | 2809 | 0 |
| logged-in-reader | desktop | profile | 3 | 29.3 | 3006 | 0 |
| logged-in-reader | desktop | reader | 1 | 28.0 | 2706 | 0 |
| logged-in-reader | desktop | readers | 7 | 28.0 | 2748 | 0 |
| logged-in-reader | desktop | reading | 5 | 28.4 | 2818 | 0 |
| logged-in-reader | desktop | wallet | 4 | 36.0 | 3122 | 0 |
| logged-in-reader | mobile | chat | 1 | 28.0 | 2788 | 0 |
| logged-in-reader | mobile | collection | 1 | 29.0 | 5603 | 0 |
| logged-in-reader | mobile | community | 1 | 30.0 | 3548 | 0 |
| logged-in-reader | mobile | gacha | 2 | 33.5 | 2980 | 0 |
| logged-in-reader | mobile | gamification | 1 | 29.0 | 2849 | 1 |
| logged-in-reader | mobile | home | 1 | 29.0 | 3064 | 0 |
| logged-in-reader | mobile | inventory | 1 | 36.0 | 2908 | 0 |
| logged-in-reader | mobile | leaderboard | 1 | 31.0 | 2796 | 0 |
| logged-in-reader | mobile | legal | 3 | 25.0 | 2732 | 0 |
| logged-in-reader | mobile | notifications | 1 | 28.0 | 2784 | 0 |
| logged-in-reader | mobile | profile | 3 | 31.7 | 3141 | 0 |
| logged-in-reader | mobile | reader | 1 | 28.0 | 2760 | 0 |
| logged-in-reader | mobile | readers | 7 | 28.0 | 2776 | 0 |
| logged-in-reader | mobile | reading | 5 | 29.2 | 2848 | 0 |
| logged-in-reader | mobile | wallet | 4 | 34.5 | 3971 | 0 |
| logged-out | desktop | auth | 5 | 24.2 | 3085 | 0 |
| logged-out | desktop | home | 1 | 29.0 | 4568 | 0 |
| logged-out | desktop | legal | 3 | 25.0 | 2895 | 0 |
| logged-out | mobile | auth | 5 | 24.2 | 2709 | 0 |
| logged-out | mobile | home | 1 | 30.0 | 3274 | 0 |
| logged-out | mobile | legal | 3 | 25.0 | 3128 | 0 |

## Per-Page Metrics
| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |
| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | auth-public | /vi | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4568 | 2384 | 2554 | 1992 | 2576 | 0.0000 | 147.0 | 601502 |
| logged-out | desktop | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2702 | 694 | 694 | 620 | 620 | 0.0000 | 0.0 | 512353 |
| logged-out | desktop | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2807 | 801 | 801 | 584 | 584 | 0.0000 | 0.0 | 513491 |
| logged-out | desktop | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3466 | 716 | 1458 | 676 | 676 | 0.0000 | 0.0 | 512087 |
| logged-out | desktop | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2741 | 732 | 733 | 656 | 656 | 0.0000 | 0.0 | 512074 |
| logged-out | desktop | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3707 | 761 | 1700 | 720 | 720 | 0.0000 | 0.0 | 512275 |
| logged-out | desktop | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3074 | 745 | 1062 | 628 | 628 | 0.0000 | 0.0 | 526002 |
| logged-out | desktop | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2692 | 683 | 684 | 620 | 620 | 0.0000 | 0.0 | 525928 |
| logged-out | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2920 | 706 | 911 | 648 | 648 | 0.0000 | 0.0 | 526092 |
| logged-in-admin | desktop | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2694 | 687 | 687 | 684 | 1220 | 0.0039 | 194.0 | 537795 |
| logged-in-admin | desktop | reading | /vi/reading | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2847 | 826 | 840 | 616 | 1008 | 0.0041 | 0.0 | 641703 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 33 | 8 | high | 0 | 0 | 2 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 3112 | 1102 | 1102 | 632 | 1292 | 0.0041 | 0.0 | 647814 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 36 | 6 | critical | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2970 | 801 | 958 | 636 | 1044 | 0.0041 | 0.0 | 733794 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2854 | 845 | 845 | 624 | 1268 | 0.0041 | 0.0 | 727137 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | 39 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6560 | 746 | 761 | 576 | 576 | 0.0042 | 16.0 | 643179 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 32 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3093 | 892 | 1086 | 608 | 984 | 0.0489 | 0.0 | 637395 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2835 | 805 | 827 | 652 | 1052 | 0.0041 | 0.0 | 631440 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 40 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2797 | 820 | 1169 | 696 | 1056 | 0.0538 | 0.0 | 630911 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2811 | 807 | 807 | 612 | 1040 | 0.0041 | 0.0 | 634047 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2847 | 833 | 839 | 624 | 1028 | 0.0041 | 0.0 | 631924 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2887 | 795 | 877 | 860 | 860 | 0.0179 | 0.0 | 649656 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 34 | 5 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3774 | 758 | 1009 | 592 | 1700 | 0.0041 | 0.0 | 776415 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 29 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3182 | 1032 | 1174 | 1044 | 1416 | 0.0176 | 0.0 | 642064 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 27 | 36 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2824 | 986 | 987 | 964 | 1548 | 0.0000 | 57.0 | 607137 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 80 | 13 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4578 | 768 | 768 | 628 | 1192 | 0.0039 | 88.0 | 1635118 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2848 | 828 | 841 | 628 | 1160 | 0.0041 | 0.0 | 632650 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | 41 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2795 | 906 | 911 | 652 | 1196 | 0.0041 | 0.0 | 631093 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2896 | 873 | 888 | 808 | 1196 | 0.0041 | 0.0 | 632244 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2753 | 733 | 745 | 728 | 1096 | 0.0041 | 0.0 | 632519 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2887 | 860 | 878 | 608 | 1260 | 0.0041 | 0.0 | 633048 |
| logged-in-admin | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2736 | 713 | 727 | 580 | 912 | 0.0020 | 0.0 | 526397 |
| logged-in-admin | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2748 | 740 | 740 | 576 | 956 | 0.0020 | 0.0 | 526074 |
| logged-in-admin | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2768 | 761 | 761 | 644 | 968 | 0.0020 | 0.0 | 526465 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2808 | 780 | 800 | 600 | 964 | 0.0000 | 0.0 | 647734 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2846 | 837 | 837 | 596 | 996 | 0.0000 | 0.0 | 647799 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2810 | 797 | 801 | 680 | 996 | 0.0000 | 0.0 | 646006 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 30 | 4 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2764 | 741 | 758 | 580 | 876 | 0.0022 | 0.0 | 664965 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2901 | 889 | 891 | 604 | 988 | 0.0000 | 0.0 | 644813 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 728 | 748 | 620 | 956 | 0.0000 | 0.0 | 646606 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2733 | 725 | 726 | 608 | 1048 | 0.0000 | 0.0 | 648808 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | 3 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2837 | 813 | 828 | 600 | 992 | 0.0000 | 0.0 | 655926 |
| logged-in-admin | desktop | admin | /vi/admin/users | 32 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3247 | 717 | 1240 | 612 | 988 | 0.0000 | 0.0 | 652610 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2842 | 823 | 837 | 608 | 976 | 0.0000 | 0.0 | 646141 |
| logged-in-admin | desktop | reading | /vi/reading/session/0d8d702c-add9-496d-abff-004e959899ad | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2785 | 753 | 775 | 616 | 1008 | 0.0044 | 0.0 | 632628 |
| logged-in-admin | desktop | reading | /vi/reading/session/ada274fc-e8cb-49ce-b67a-036ee80a3f60 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2821 | 792 | 813 | 608 | 1008 | 0.0041 | 0.0 | 632236 |
| logged-in-admin | desktop | reading | /vi/reading/session/20b2e441-f966-48fc-8c86-592f9aa381d9 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2682 | 656 | 674 | 628 | 968 | 0.0041 | 0.0 | 632367 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2720 | 658 | 709 | 572 | 964 | 0.0041 | 0.0 | 631274 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2769 | 751 | 760 | 608 | 992 | 0.0041 | 0.0 | 631381 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2919 | 864 | 910 | 592 | 976 | 0.0041 | 0.0 | 631363 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2763 | 735 | 754 | 732 | 1280 | 0.0041 | 0.0 | 633036 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2809 | 773 | 801 | 860 | 1024 | 0.0041 | 0.0 | 633350 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2788 | 760 | 778 | 600 | 1112 | 0.0041 | 0.0 | 633213 |
| logged-in-reader | desktop | auth-public | /vi | 29 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3142 | 771 | 1134 | 624 | 1196 | 0.0038 | 50.0 | 608072 |
| logged-in-reader | desktop | reading | /vi/reading | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3024 | 991 | 1015 | 644 | 1100 | 0.0039 | 0.0 | 644041 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 31 | 10 | high | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2841 | 800 | 830 | 604 | 1044 | 0.0039 | 0.0 | 645834 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 36 | 6 | critical | 0 | 0 | 1 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2927 | 887 | 892 | 624 | 1028 | 0.0039 | 0.0 | 733810 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3003 | 826 | 996 | 644 | 1220 | 0.0039 | 0.0 | 725406 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | 32 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6906 | 830 | 1198 | 624 | 996 | 0.0040 | 0.0 | 642369 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 32 | 5 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2952 | 797 | 940 | 604 | 1044 | 0.0726 | 0.0 | 637735 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2738 | 708 | 730 | 600 | 996 | 0.0039 | 0.0 | 631738 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3327 | 769 | 1317 | 740 | 1132 | 0.0039 | 0.0 | 632390 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2848 | 838 | 838 | 696 | 1060 | 0.0039 | 0.0 | 634000 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3033 | 722 | 1024 | 680 | 1084 | 0.0039 | 0.0 | 631617 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 33 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3194 | 781 | 1185 | 672 | 1044 | 0.0177 | 0.0 | 653446 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3479 | 716 | 743 | 568 | 1716 | 0.0039 | 0.0 | 643343 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3036 | 811 | 1026 | 596 | 1012 | 0.1238 | 0.0 | 642262 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 59 | 2 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4197 | 1306 | 1489 | 996 | 1572 | 0.0000 | 25.0 | 1206601 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2765 | 739 | 756 | 664 | 1032 | 0.0039 | 0.0 | 631829 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2709 | 673 | 698 | 576 | 984 | 0.0039 | 0.0 | 632100 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2815 | 806 | 806 | 620 | 1000 | 0.0095 | 0.0 | 634150 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2809 | 797 | 797 | 584 | 988 | 0.0040 | 0.0 | 633754 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2706 | 676 | 695 | 588 | 960 | 0.0039 | 0.0 | 632693 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2703 | 674 | 692 | 616 | 1132 | 0.0039 | 0.0 | 632954 |
| logged-in-reader | desktop | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2803 | 793 | 793 | 568 | 884 | 0.0019 | 0.0 | 526321 |
| logged-in-reader | desktop | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2686 | 676 | 676 | 644 | 932 | 0.0019 | 0.0 | 526228 |
| logged-in-reader | desktop | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2650 | 639 | 639 | 600 | 912 | 0.0019 | 0.0 | 526502 |
| logged-in-reader | desktop | reading | /vi/reading/session/c653549e-cb42-4082-8ffa-945e18b841a6 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2820 | 788 | 810 | 624 | 1004 | 0.0039 | 0.0 | 632530 |
| logged-in-reader | desktop | reading | /vi/reading/session/b66b45c8-1c4b-4774-83ee-dc635e33a289 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2715 | 701 | 706 | 576 | 1012 | 0.0039 | 0.0 | 632364 |
| logged-in-reader | desktop | reading | /vi/reading/session/e2171e9a-edc9-45ca-bfa8-1a7fd5921169 | 28 | 11 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2828 | 802 | 818 | 648 | 1076 | 0.0039 | 0.0 | 632237 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2763 | 730 | 753 | 588 | 1012 | 0.0039 | 0.0 | 631387 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2723 | 692 | 711 | 604 | 968 | 0.0039 | 0.0 | 631648 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2771 | 738 | 761 | 664 | 1028 | 0.0039 | 0.0 | 631304 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2691 | 663 | 680 | 616 | 1136 | 0.0039 | 0.0 | 633178 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2646 | 613 | 636 | 580 | 1136 | 0.0039 | 0.0 | 632831 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2796 | 768 | 784 | 596 | 1164 | 0.0039 | 0.0 | 633263 |
| logged-out | mobile | auth-public | /vi | 30 | 1 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3274 | 1249 | 1266 | 1076 | 1076 | 0.0000 | 0.0 | 602825 |
| logged-out | mobile | auth-public | /vi/login | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2804 | 791 | 791 | 652 | 652 | 0.0000 | 0.0 | 512325 |
| logged-out | mobile | auth-public | /vi/register | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2642 | 630 | 631 | 564 | 564 | 0.0000 | 0.0 | 513610 |
| logged-out | mobile | auth-public | /vi/forgot-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2697 | 617 | 687 | 560 | 560 | 0.0000 | 0.0 | 512061 |
| logged-out | mobile | auth-public | /vi/reset-password | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2654 | 646 | 646 | 548 | 548 | 0.0000 | 0.0 | 512166 |
| logged-out | mobile | auth-public | /vi/verify-email | 24 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2747 | 733 | 733 | 572 | 572 | 0.0000 | 0.0 | 512107 |
| logged-out | mobile | auth-public | /vi/legal/tos | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3069 | 699 | 1054 | 560 | 560 | 0.0000 | 0.0 | 525836 |
| logged-out | mobile | auth-public | /vi/legal/privacy | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2807 | 791 | 791 | 616 | 616 | 0.0000 | 0.0 | 525923 |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 1 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3507 | 727 | 1492 | 624 | 624 | 0.0000 | 0.0 | 526023 |
| logged-in-admin | mobile | auth-public | /vi | 26 | 13 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3456 | 1371 | 1446 | 1376 | 1736 | 0.0032 | 0.0 | 537734 |
| logged-in-admin | mobile | reading | /vi/reading | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3399 | 1156 | 1390 | 928 | 1268 | 0.0000 | 0.0 | 645146 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 37 | 3 | critical | 0 | 0 | 1 | 0 | 5 | 5 | 0 | 5 | 0 | 0 | 3554 | 1376 | 1544 | 1028 | 1364 | 0.0000 | 0.0 | 663688 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 35 | 6 | high | 0 | 0 | 0 | 0 | 5 | 4 | 1 | 5 | 0 | 0 | 3579 | 1256 | 1569 | 932 | 1288 | 0.0071 | 0.0 | 797083 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3546 | 1399 | 1537 | 1128 | 1488 | 0.0000 | 0.0 | 726271 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 30 | 37 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 6220 | 1021 | 1256 | 932 | 1260 | 0.0000 | 0.0 | 644385 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3824 | 1455 | 1812 | 792 | 1408 | 0.0760 | 0.0 | 636666 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3896 | 1499 | 1885 | 900 | 1244 | 0.0071 | 0.0 | 631713 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | 36 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4650 | 1687 | - | 856 | 1500 | 0.0760 | 0.0 | 631121 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4850 | 1624 | 2837 | 1576 | 1948 | 0.0071 | 0.0 | 634101 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3479 | 1291 | 1469 | 760 | 1112 | 0.0071 | 0.0 | 631980 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4211 | 1376 | 2201 | 804 | 1132 | 0.0267 | 0.0 | 649832 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 38 | 2 | critical | 0 | 0 | 1 | 0 | 1 | 0 | 1 | 1 | 0 | 0 | 4866 | 955 | 2131 | 900 | 2168 | 0.0051 | 0.0 | 788743 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 30 | 7 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3310 | 1299 | 1299 | 932 | 1300 | 0.0000 | 0.0 | 643179 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3325 | 1315 | 1316 | 912 | 1568 | 0.0000 | 0.0 | 637521 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 29 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3240 | 1024 | 1230 | 932 | 1272 | 0.0000 | 0.0 | 632634 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3295 | 1195 | 1284 | 1180 | 1572 | 0.0000 | 0.0 | 632243 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 37 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3225 | 1172 | 1205 | 1000 | 1344 | 0.0071 | 0.0 | 632036 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4940 | 2565 | 2928 | 2544 | 2544 | 0.0000 | 0.0 | 632105 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 31 | 3 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3291 | 1068 | 1283 | 864 | 1192 | 0.0000 | 0.0 | 635881 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3756 | 1485 | 1747 | 984 | 984 | 0.0071 | 0.0 | 633158 |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3660 | 1382 | 1650 | 868 | 1184 | 0.0055 | 0.0 | 526311 |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3904 | 1558 | 1893 | 976 | 1304 | 0.0055 | 0.0 | 526384 |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3569 | 1357 | 1560 | 828 | 1140 | 0.0055 | 0.0 | 526472 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3380 | 1111 | 1368 | 1084 | 1412 | 0.0000 | 0.0 | 647734 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3684 | 1529 | 1674 | 880 | 1204 | 0.0000 | 0.0 | 647853 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3816 | 1364 | 1806 | 844 | 1160 | 0.0000 | 0.0 | 646978 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 33 | 1 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3690 | 1212 | 1679 | 1112 | 1452 | 0.0030 | 0.0 | 699657 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3408 | 1160 | 1398 | 1024 | 1364 | 0.0000 | 0.0 | 644773 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3601 | 1312 | 1590 | 736 | 1056 | 0.0000 | 0.0 | 646519 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 30 | 2 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3508 | 1277 | 1500 | 1112 | 1440 | 0.0000 | 0.0 | 649883 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 29 | 2 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3788 | 1533 | 1772 | 880 | 1212 | 0.0000 | 0.0 | 687815 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | 2 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 4064 | 1447 | 2052 | 844 | 1168 | 0.0000 | 0.0 | 650962 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 30 | 2 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3468 | 1233 | 1457 | 1036 | 1356 | 0.0000 | 0.0 | 647228 |
| logged-in-admin | mobile | reading | /vi/reading/session/3c5f4cf7-262a-4376-92d7-93372ce77f3d | 30 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3783 | 1585 | 1775 | 872 | 1224 | 0.0072 | 0.0 | 680884 |
| logged-in-admin | mobile | reading | /vi/reading/session/e9de656d-e234-41cc-82b3-935832ca545a | 29 | 9 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3300 | 1101 | 1288 | 1020 | 1352 | 0.0001 | 0.0 | 633389 |
| logged-in-admin | mobile | reading | /vi/reading/session/0a05af9d-c4bd-43e4-b2a7-a3a96b72b96c | 30 | 7 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3787 | 1672 | 1776 | 824 | 1160 | 0.0072 | 0.0 | 681090 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3918 | 1573 | 1905 | 980 | 980 | 0.0071 | 0.0 | 633298 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3724 | 1355 | 1714 | 832 | 1152 | 0.0071 | 0.0 | 631281 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3721 | 1465 | 1708 | 1004 | 1004 | 0.0071 | 0.0 | 631144 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3278 | 1117 | 1268 | 1096 | 1516 | 0.0000 | 0.0 | 632734 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3662 | 1474 | 1649 | 1484 | 1520 | 0.0000 | 0.0 | 633064 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3769 | 1483 | 1757 | 816 | 1164 | 0.0071 | 0.0 | 633113 |
| logged-in-reader | mobile | auth-public | /vi | 29 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3064 | 762 | 1053 | 528 | 884 | 0.0055 | 0.0 | 608040 |
| logged-in-reader | mobile | reading | /vi/reading | 28 | 5 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3017 | 858 | 1004 | 616 | 948 | 0.0000 | 0.0 | 642179 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 36 | 5 | critical | 0 | 0 | 1 | 0 | 5 | 0 | 0 | 5 | 0 | 0 | 2908 | 851 | 873 | 572 | 912 | 0.0000 | 0.0 | 662727 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 36 | 6 | critical | 0 | 0 | 1 | 0 | 5 | 1 | 0 | 5 | 0 | 0 | 2995 | 869 | 983 | 596 | 952 | 0.0000 | 0.0 | 798419 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2965 | 866 | 955 | 596 | 932 | 0.0000 | 0.0 | 725251 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | 31 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 5603 | 741 | 752 | 744 | 744 | 0.0000 | 0.0 | 642332 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | 4 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2927 | 818 | 917 | 540 | 876 | 0.0000 | 0.0 | 638970 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2880 | 851 | 867 | 792 | 1128 | 0.0000 | 0.0 | 631921 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 34 | 1 | high | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3616 | 729 | 1600 | 560 | 888 | 0.0000 | 0.0 | 646878 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2830 | 814 | 814 | 540 | 872 | 0.0000 | 0.0 | 634091 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2788 | 750 | 769 | 584 | 920 | 0.0000 | 0.0 | 632080 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 31 | 5 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2796 | 683 | 784 | 532 | 860 | 0.0196 | 0.0 | 651062 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | 10 | high | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 1 | 0 | 0 | 3548 | 803 | 807 | 568 | 1688 | 0.0051 | 0.0 | 643515 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | 9 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2849 | 833 | 833 | 568 | 904 | 0.0000 | 0.0 | 642358 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 27 | 36 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2810 | 967 | 1362 | 972 | 972 | 0.0000 | 0.0 | 607184 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 54 | 41 | critical | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 7451 | 783 | 783 | 572 | 944 | 0.0055 | 0.0 | 1109339 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2783 | 769 | 770 | 576 | 908 | 0.0000 | 0.0 | 632003 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | 6 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2841 | 829 | 832 | 536 | 884 | 0.0330 | 0.0 | 634473 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2784 | 761 | 770 | 656 | 988 | 0.0000 | 0.0 | 632331 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2760 | 656 | 749 | 528 | 872 | 0.0000 | 0.0 | 632800 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2726 | 695 | 713 | 540 | 952 | 0.0000 | 0.0 | 633182 |
| logged-in-reader | mobile | auth-public | /vi/legal/tos | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2757 | 742 | 742 | 576 | 896 | 0.0032 | 0.0 | 526356 |
| logged-in-reader | mobile | auth-public | /vi/legal/privacy | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2677 | 663 | 663 | 536 | 860 | 0.0032 | 0.0 | 526351 |
| logged-in-reader | mobile | auth-public | /vi/legal/ai-disclaimer | 25 | 3 | none | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2762 | 629 | 751 | 556 | 864 | 0.0032 | 0.0 | 526537 |
| logged-in-reader | mobile | reading | /vi/reading/session/aacf512a-e609-431b-8466-4dd992f3b0a3 | 34 | 3 | high | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 3017 | 690 | 1006 | 536 | 880 | 0.0000 | 0.0 | 695041 |
| logged-in-reader | mobile | reading | /vi/reading/session/6b7e5e64-9b9e-47f1-922f-dcc11f5225fe | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2780 | 751 | 769 | 572 | 908 | 0.0000 | 0.0 | 632542 |
| logged-in-reader | mobile | reading | /vi/reading/session/11225fc7-e35a-4253-b23f-ba0cf8c37b36 | 28 | 10 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2702 | 667 | 687 | 560 | 888 | 0.0000 | 0.0 | 632449 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2784 | 754 | 772 | 632 | 968 | 0.0000 | 0.0 | 631325 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2767 | 735 | 756 | 544 | 876 | 0.0000 | 0.0 | 631501 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2753 | 725 | 738 | 540 | 868 | 0.0000 | 0.0 | 631516 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2780 | 754 | 767 | 544 | 960 | 0.0000 | 0.0 | 633129 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2742 | 708 | 728 | 564 | 976 | 0.0000 | 0.0 | 633042 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | 6 | high | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2775 | 740 | 761 | 552 | 904 | 0.0000 | 0.0 | 633061 |

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
| logged-in-admin | desktop | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | desktop | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/inventory | 33 | high | 3 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | 36 | critical | 1 | 33 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | 32 | high | 3 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | 34 | high | 0 | 32 | 0 |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet | 27 | high | 0 | 25 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit | 80 | critical | 3 | 71 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/disputes | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/gamification | 30 | high | 0 | 28 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/readings | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/users | 32 | high | 1 | 29 | 0 |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | 29 | high | 0 | 27 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/0d8d702c-add9-496d-abff-004e959899ad | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/ada274fc-e8cb-49ce-b67a-036ee80a3f60 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reading | /vi/reading/session/20b2e441-f966-48fc-8c86-592f9aa381d9 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | reading | /vi/reading | 30 | high | 2 | 26 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | 31 | high | 1 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | 36 | critical | 1 | 33 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | 32 | high | 3 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | 33 | high | 3 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet | 59 | critical | 3 | 52 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | 29 | high | 1 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/c653549e-cb42-4082-8ffa-945e18b841a6 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/b66b45c8-1c4b-4774-83ee-dc635e33a289 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reading | /vi/reading/session/e2171e9a-edc9-45ca-bfa8-1a7fd5921169 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | desktop | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-out | mobile | auth-public | /vi | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | auth-public | /vi | 26 | high | 0 | 24 | 0 |
| logged-in-admin | mobile | reading | /vi/reading | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | 37 | critical | 2 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | 35 | high | 0 | 33 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/collection | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | 31 | high | 2 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | 38 | critical | 3 | 33 | 0 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/reader/apply | 31 | high | 3 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/deposits | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/disputes | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/gamification | 33 | high | 1 | 30 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/promotions | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/reader-requests | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/readings | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | 29 | high | 0 | 27 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/users | 31 | high | 1 | 28 | 0 |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | 30 | high | 1 | 27 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/3c5f4cf7-262a-4376-92d7-93372ce77f3d | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/e9de656d-e234-41cc-82b3-935832ca545a | 29 | high | 1 | 26 | 0 |
| logged-in-admin | mobile | reading | /vi/reading/session/0a05af9d-c4bd-43e4-b2a7-a3a96b72b96c | 30 | high | 0 | 28 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 30 | high | 2 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | auth-public | /vi | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | reading | /vi/reading | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/inventory | 36 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha | 36 | critical | 1 | 33 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/gacha/history | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | inventory-gacha-collection | /vi/collection | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | 33 | high | 3 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | 34 | high | 4 | 27 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/chat | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/leaderboard | 31 | high | 1 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/community | 30 | high | 0 | 28 | 0 |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | 29 | high | 0 | 27 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet | 27 | high | 0 | 25 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | 54 | critical | 3 | 47 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | 29 | high | 1 | 26 | 0 |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/reader/apply | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/history | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/aacf512a-e609-431b-8466-4dd992f3b0a3 | 34 | high | 3 | 29 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/6b7e5e64-9b9e-47f1-922f-dcc11f5225fe | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reading | /vi/reading/session/11225fc7-e35a-4253-b23f-ba0cf8c37b36 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | 28 | high | 0 | 26 | 0 |
| logged-in-reader | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | 28 | high | 0 | 26 | 0 |

## High Slow Requests (>800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/deposit | GET | 200 | 4121 | 2805 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 2000 | 1571 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1829 | 1478 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-out | desktop | auth-public | /vi | GET | 200 | 1695 | 879 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 1650 | 946 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 1546 | 763 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 1541 | 602 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 1518 | 525 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1406 | 644 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 1348 | 725 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-out | desktop | auth-public | /vi/verify-email | GET | 200 | 1333 | 288 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 1315 | 298 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1308 | 417 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/inventory | GET | 200 | 1292 | 610 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 1264 | 1245 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | reader-chat | /vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf | GET | 200 | 1262 | 1063 | html | https://www.tarotnow.xyz/vi/readers/40c8fbd5-9484-475e-8713-37d6a41a5bcf |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/wallet | GET | 200 | 1260 | 540 | html | https://www.tarotnow.xyz/vi/wallet |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 1257 | 242 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1253 | 264 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/community | GET | 200 | 1252 | 568 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1200&q=75 |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 1239 | 534 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1239 | 340 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 1238 | 272 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 1237 | 220 | static | https://www.tarotnow.xyz/_next/static/chunks/02u6sgqed9rhs.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1233 | 345 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1226 | 546 | html | https://www.tarotnow.xyz/vi/legal/privacy |
| logged-in-admin | mobile | admin | /vi/admin/readings | GET | 200 | 1223 | 563 | html | https://www.tarotnow.xyz/vi/admin/readings |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1206 | 334 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | reading | /vi/reading/session/3c5f4cf7-262a-4376-92d7-93372ce77f3d | GET | 200 | 1201 | 293 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | inventory-gacha-collection | /vi/gacha | GET | 200 | 1197 | 541 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 1196 | 654 | static | https://www.tarotnow.xyz/_next/static/chunks/0b5a588g_0r8q.js |
| logged-in-admin | mobile | reading | /vi/reading/session/3c5f4cf7-262a-4376-92d7-93372ce77f3d | GET | 200 | 1193 | 328 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1190 | 244 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | reading | /vi/reading/session/0a05af9d-c4bd-43e4-b2a7-a3a96b72b96c | GET | 200 | 1189 | 342 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1189 | 231 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | reading | /vi/reading/session/0a05af9d-c4bd-43e4-b2a7-a3a96b72b96c | GET | 200 | 1187 | 343 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | admin | /vi/admin/withdrawals | GET | 200 | 1175 | 564 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1172 | 276 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | reading | /vi/reading/session/0a05af9d-c4bd-43e4-b2a7-a3a96b72b96c | GET | 200 | 1169 | 308 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1161 | 294 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 1160 | 189 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | reading | /vi/reading/session/3c5f4cf7-262a-4376-92d7-93372ce77f3d | GET | 200 | 1150 | 223 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-out | desktop | auth-public | /vi/forgot-password | GET | 200 | 1146 | 299 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 1141 | 260 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | admin | /vi/admin/gamification | GET | 200 | 1140 | 548 | html | https://www.tarotnow.xyz/vi/admin/gamification |
| logged-in-admin | mobile | reading | /vi/reading/session/0a05af9d-c4bd-43e4-b2a7-a3a96b72b96c | GET | 200 | 1135 | 267 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | reading | /vi/reading/session/3c5f4cf7-262a-4376-92d7-93372ce77f3d | GET | 200 | 1129 | 246 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1123 | 273 | static | https://www.tarotnow.xyz/_next/static/chunks/0rm0_6_wsunhe.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 1115 | 478 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 1115 | 211 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 1115 | 305 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 1114 | 307 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 1113 | 293 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-out | mobile | auth-public | /vi | GET | 200 | 1105 | 595 | html | https://www.tarotnow.xyz/vi |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 1099 | 300 | static | https://www.tarotnow.xyz/_next/static/chunks/17ovlvq8jtm.c.js |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 1095 | 297 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | admin | /vi/admin/promotions | GET | 200 | 1095 | 548 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 1093 | 312 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1092 | 216 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 1088 | 191 | static | https://www.tarotnow.xyz/_next/static/chunks/0haxg9p1~c1ud.js |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 1086 | 186 | static | https://www.tarotnow.xyz/_next/static/chunks/02u6sgqed9rhs.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 1083 | 290 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1082 | 264 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | reading | /vi/reading | GET | 200 | 1078 | 483 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | auth-public | /vi/legal/tos | GET | 200 | 1078 | 428 | static | https://www.tarotnow.xyz/_next/static/chunks/0rm0_6_wsunhe.js |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1077 | 334 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 1077 | 341 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1072 | 236 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 1067 | 274 | static | https://www.tarotnow.xyz/_next/static/chunks/02u6sgqed9rhs.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1067 | 319 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 1066 | 267 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 1062 | 301 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 1058 | 307 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 1055 | 277 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 1052 | 304 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 1052 | 251 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 1051 | 214 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-admin | mobile | auth-public | /vi/legal/privacy | GET | 200 | 1050 | 326 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 1048 | 331 | static | https://www.tarotnow.xyz/_next/static/chunks/17ovlvq8jtm.c.js |
| logged-out | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 1047 | 949 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |

## Medium Slow Requests (400-800ms)
| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |
| logged-out | desktop | auth-public | /vi | GET | 200 | 800 | 442 | static | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 800 | 228 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 799 | 319 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | mobile | reading | /vi/reading/session/3c5f4cf7-262a-4376-92d7-93372ce77f3d | GET | 200 | 798 | 226 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 798 | 236 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 797 | 269 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 797 | 455 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | desktop | admin | /vi/admin/promotions | GET | 200 | 796 | 323 | html | https://www.tarotnow.xyz/vi/admin/promotions |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 796 | 225 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | admin | /vi/admin/promotions | GET | 200 | 796 | 792 | static | https://www.tarotnow.xyz/_next/static/chunks/153.59amsfq4f.js |
| logged-in-admin | mobile | reading | /vi/reading/session/0a05af9d-c4bd-43e4-b2a7-a3a96b72b96c | GET | 200 | 796 | 235 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 795 | 334 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 795 | 782 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 795 | 270 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/97410d49-ab32-469d-b421-f57321b262a2 | GET | 200 | 793 | 317 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 792 | 331 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | mobile | reading | /vi/reading/history | GET | 200 | 792 | 180 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 789 | 246 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/gacha/history | GET | 200 | 788 | 335 | html | https://www.tarotnow.xyz/vi/gacha/history |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 785 | 316 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/wallet/deposit/history | GET | 200 | 785 | 327 | html | https://www.tarotnow.xyz/vi/wallet/deposit/history |
| logged-in-reader | desktop | reader-chat | /vi/readers | GET | 200 | 784 | 418 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 784 | 274 | static | https://www.tarotnow.xyz/_next/static/chunks/17c_y7i3dw8__.js |
| logged-in-admin | desktop | reading | /vi/reading | GET | 200 | 782 | 313 | html | https://www.tarotnow.xyz/vi/reading |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 782 | 290 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-reader | mobile | community-leaderboard-quest | /vi/gamification | GET | 200 | 782 | 326 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 781 | 203 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | desktop | admin | /vi/admin/withdrawals | GET | 200 | 780 | 324 | html | https://www.tarotnow.xyz/vi/admin/withdrawals |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 779 | 271 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-out | desktop | auth-public | /vi | GET | 200 | 778 | 646 | static | https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css |
| logged-in-admin | mobile | admin | /vi/admin/system-configs | GET | 200 | 777 | 205 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 777 | 289 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | auth-public | /vi | GET | 200 | 776 | 431 | static | https://www.tarotnow.xyz/_next/static/media/7178b3e590c64307-s.p.11.cyxs5p-0z~.woff2 |
| logged-in-admin | mobile | reader-chat | /vi/readers/69e7b9fa5b238c89fadb3a0b | GET | 200 | 774 | 188 | static | https://www.tarotnow.xyz/_next/static/chunks/123t3kd09_1tj.js |
| logged-in-admin | mobile | reading | /vi/reading/session/0a05af9d-c4bd-43e4-b2a7-a3a96b72b96c | GET | 200 | 773 | 218 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-reader | desktop | community-leaderboard-quest | /vi/gamification | GET | 200 | 772 | 316 | html | https://www.tarotnow.xyz/vi/gamification |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 772 | 314 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 771 | 292 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 771 | 517 | static | https://www.tarotnow.xyz/_next/static/chunks/07or0in3dm_w_.js |
| logged-in-admin | mobile | reading | /vi/reading/session/0a05af9d-c4bd-43e4-b2a7-a3a96b72b96c | GET | 200 | 771 | 46 | telemetry | https://static.cloudflareinsights.com/beacon.min.js/v8c78df7c7c0f484497ecbca7046644da1771523124516 |
| logged-in-admin | desktop | admin | /vi/admin/system-configs | GET | 200 | 770 | 309 | html | https://www.tarotnow.xyz/vi/admin/system-configs |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 770 | 303 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | desktop | reader-chat | /vi/chat | GET | 200 | 769 | 336 | html | https://www.tarotnow.xyz/vi/chat |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 769 | 258 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-reader | mobile | reader-chat | /vi/readers | GET | 200 | 769 | 312 | html | https://www.tarotnow.xyz/vi/readers |
| logged-out | desktop | auth-public | /vi | GET | 200 | 768 | 443 | static | https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 768 | 145 | static | https://www.tarotnow.xyz/_next/static/chunks/0pwly2ey-ozy~.js |
| logged-in-admin | desktop | reader-chat | /vi/readers | GET | 200 | 767 | 316 | html | https://www.tarotnow.xyz/vi/readers |
| logged-in-admin | desktop | admin | /vi/admin/deposits | GET | 200 | 767 | 317 | html | https://www.tarotnow.xyz/vi/admin/deposits |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/wallet/withdraw | GET | 200 | 767 | 308 | html | https://www.tarotnow.xyz/vi/wallet/withdraw |
| logged-in-admin | mobile | auth-public | /vi | GET | 200 | 766 | 445 | static | https://www.tarotnow.xyz/_next/static/chunks/147.fw.lfq60x.css |
| logged-in-admin | mobile | reading | /vi/reading/session/3c5f4cf7-262a-4376-92d7-93372ce77f3d | GET | 200 | 766 | 180 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 765 | 273 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/profile | GET | 200 | 764 | 307 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | auth-public | /vi/legal/ai-disclaimer | GET | 200 | 763 | 300 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 762 | 274 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-reader | mobile | profile-wallet-notifications | /vi/notifications | GET | 200 | 761 | 308 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | desktop | inventory-gacha-collection | /vi/gacha | GET | 200 | 760 | 302 | html | https://www.tarotnow.xyz/vi/gacha |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/inventory | GET | 200 | 760 | 304 | html | https://www.tarotnow.xyz/vi/inventory |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/profile | GET | 200 | 758 | 301 | html | https://www.tarotnow.xyz/vi/profile |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/reader | GET | 200 | 758 | 289 | static | https://www.tarotnow.xyz/_next/static/chunks/0x.73w57rn4ou.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69d93c24bc68b27090414f6c | GET | 200 | 758 | 234 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-reader | desktop | profile-wallet-notifications | /vi/notifications | GET | 200 | 757 | 305 | html | https://www.tarotnow.xyz/vi/notifications |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 757 | 293 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | desktop | community-leaderboard-quest | /vi/community | GET | 200 | 756 | 289 | static | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Fcommunity%2F2ef125e575c84b8d990e4c90ed64d5f3.webp&w=1920&q=75 |
| logged-in-admin | mobile | reader-chat | /vi/readers | GET | 200 | 756 | 198 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-reader | desktop | reading | /vi/reading/session/e2171e9a-edc9-45ca-bfa8-1a7fd5921169 | GET | 200 | 755 | 322 | html | https://www.tarotnow.xyz/vi/reading/session/e2171e9a-edc9-45ca-bfa8-1a7fd5921169 |
| logged-in-admin | mobile | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 755 | 271 | static | https://www.tarotnow.xyz/_next/static/chunks/0kj4sun3~h8xd.js |
| logged-in-admin | mobile | reader-chat | /vi/chat | GET | 200 | 755 | 204 | static | https://www.tarotnow.xyz/_next/static/chunks/0umwwbfv0qsm4.js |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 755 | 200 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 754 | 225 | static | https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js |
| logged-in-admin | mobile | admin | /vi/admin/disputes | GET | 200 | 754 | 183 | static | https://www.tarotnow.xyz/_next/static/chunks/0b~~meelw632j.js |
| logged-in-admin | mobile | admin | /vi/admin/users | GET | 200 | 754 | 175 | static | https://www.tarotnow.xyz/_next/static/chunks/0h5.6moaq6wat.js |
| logged-in-admin | mobile | auth-public | /vi | GET | 200 | 753 | 273 | static | https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js |
| logged-in-admin | desktop | profile-wallet-notifications | /vi/profile/mfa | GET | 200 | 751 | 331 | html | https://www.tarotnow.xyz/vi/profile/mfa |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 751 | 287 | static | https://www.tarotnow.xyz/_next/static/chunks/0a2n5k_101fwj.js |
| logged-in-admin | mobile | admin | /vi/admin/deposits | GET | 200 | 751 | 202 | static | https://www.tarotnow.xyz/_next/static/chunks/0b5a588g_0r8q.js |
| logged-in-admin | mobile | reader-chat | /vi/readers/69dbe86b052d3c8f3f55e231 | GET | 200 | 751 | 268 | static | https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js |
| logged-in-admin | mobile | community-leaderboard-quest | /vi/leaderboard | GET | 200 | 750 | 171 | static | https://www.tarotnow.xyz/_next/static/chunks/0c4xf8mjx0gok.js |
| logged-in-reader | desktop | inventory-gacha-collection | /vi/collection | GET | 200 | 749 | 315 | html | https://www.tarotnow.xyz/vi/collection |

## Pending Requests
| Scenario | Viewport | Route | URL |
| --- | --- | --- | --- |
| logged-in-admin | desktop | /vi/profile/reader | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | desktop | /vi/gamification | https://www.tarotnow.xyz/vi/gamification |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/_next/image?url=https%3A%2F%2Fui-avatars.com%2Fapi%2F%3Fbackground%3D111%26color%3Dfff%26name%3DLucifer&w=384&q=75 |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/api/me/runtime-policies |
| logged-in-admin | mobile | /vi/profile/reader | https://www.tarotnow.xyz/api/auth/session?mode=lite |
| logged-in-admin | mobile | /vi/wallet/withdraw | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | /vi/readers/69e7b9fa5b238c89fadb3a0b | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
| logged-in-admin | mobile | /vi/readers/bff951d3-ee70-43c9-99d0-aba27e81f580 | https://www.tarotnow.xyz/api/v1/presence/negotiate?negotiateVersion=1 |
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
| logged-in-admin | desktop | reading.init.spread_3: created 0d8d702c-add9-496d-abff-004e959899ad. |
| logged-in-admin | desktop | reading.init.spread_5: created ada274fc-e8cb-49ce-b67a-036ee80a3f60. |
| logged-in-admin | desktop | reading.init.spread_10: created 20b2e441-f966-48fc-8c86-592f9aa381d9. |
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
| logged-in-reader | desktop | reading.init.spread_3: created c653549e-cb42-4082-8ffa-945e18b841a6. |
| logged-in-reader | desktop | reading.init.spread_5: created b66b45c8-1c4b-4774-83ee-dc635e33a289. |
| logged-in-reader | desktop | reading.init.spread_10: created e2171e9a-edc9-45ca-bfa8-1a7fd5921169. |
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
| logged-in-admin | mobile | reading.init.spread_3: created 3c5f4cf7-262a-4376-92d7-93372ce77f3d. |
| logged-in-admin | mobile | reading.init.spread_5: created e9de656d-e234-41cc-82b3-935832ca545a. |
| logged-in-admin | mobile | reading.init.spread_10: created 0a05af9d-c4bd-43e4-b2a7-a3a96b72b96c. |
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
| logged-in-reader | mobile | reading.init.spread_3: created aacf512a-e609-431b-8466-4dd992f3b0a3. |
| logged-in-reader | mobile | reading.init.spread_5: created 6b7e5e64-9b9e-47f1-922f-dcc11f5225fe. |
| logged-in-reader | mobile | reading.init.spread_10: created 11225fc7-e35a-4253-b23f-ba0cf8c37b36. |
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
- Attempt 1: login response and route-change both failed.
- Attempt 2: login bootstrap succeeded.
