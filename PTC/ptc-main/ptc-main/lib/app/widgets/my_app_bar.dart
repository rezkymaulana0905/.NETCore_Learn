import '../constants/constants.dart';

class MyAppBar extends StatelessWidget implements PreferredSizeWidget{
  const MyAppBar({super.key, required this.title, required this.onTap});

  final String title;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    return AppBar(
      elevation: 4,
      shadowColor: Colors.black,
      title: Text(
        title,
        style: const TextStyle(fontWeight: FontWeight.w600, fontSize: 24),
      ),
      leading: GestureDetector(
        onTap: onTap,
        child: Container(
          margin: const EdgeInsets.all(12),
          width: baseWidth * 0.08,
          height: baseWidth * 0.08,
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(38),
            color: AppColor.theme,
          ),
          child: const Icon(
            Icons.arrow_back,
            color: Colors.white,
          ),
        ),
      ),
    );
  }

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);
}