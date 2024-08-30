import '../constants/constants.dart';

class SearchBox extends StatelessWidget {
  const SearchBox({super.key, required this.searchController});

  final TextEditingController searchController;

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: const EdgeInsets.only(top: 40.0, bottom: 30.0),
      alignment: Alignment.center,
      width: baseWidth * 0.6,
      height: baseHeight * 0.06,
      padding: const EdgeInsets.symmetric(horizontal: 10),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(30),
        boxShadow: const <BoxShadow>[
          BoxShadow(color: AppColor.shadow, blurRadius: 5, spreadRadius: 1),
        ],
      ),
      child: TextField(
        controller: searchController,
        textAlign: TextAlign.center,
        decoration: const InputDecoration(
          border: InputBorder.none,
          hintText: "Search Vehicle ID",
          hintStyle: TextStyle(fontSize: 16, color: AppColor.shadow),
        ),
      ),
    );
  }
}
