import '../../constants/constants.dart';
import 'home.dart';
import 'widgets/widgets.dart';

class HomePageLandscape extends GetView<HomeController> {
  const HomePageLandscape({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      body: Row(
        children: <Widget>[
          Expanded(child: _HomeBody(controller: controller)),
          Expanded(
              flex: 2,
              child: Padding(
                padding: EdgeInsets.only(top: baseHeight * 0.05),
                child: const LogActivity(),
              )),
        ],
      ),
      floatingActionButtonLocation: FloatingActionButtonLocation.endTop,
      floatingActionButton: Padding(
        padding: const EdgeInsets.only(top: 25.0, right: 10.0),
        child: _buildLogoutButton(),
      ),
    );
  }

  Widget _buildLogoutButton() {
    final isPressed = false.obs;
    return GestureDetector(
      onTapCancel: () => isPressed.value = !isPressed.value,
      onTapDown: (_) => isPressed.value = !isPressed.value,
      onTapUp: (_) {
        isPressed.value = !isPressed.value;
        controller.logout();
      },
      child: Obx(
        () => AnimatedContainer(
          duration: const Duration(milliseconds: 50),
          padding: EdgeInsets.all(baseWidth * 0.025),
          width: isPressed.value ? baseWidth * 0.1 * 0.95 : baseWidth * 0.1,
          height: isPressed.value ? baseWidth * 0.1 * 0.95 : baseWidth * 0.1,
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            color: AppColor.leave,
            boxShadow: [
              if (!isPressed.value)
                const BoxShadow(
                  color: AppColor.shadow,
                  blurRadius: 3,
                  spreadRadius: 2,
                ),
            ],
          ),
          child: const FittedBox(
            child: Icon(
              Icons.logout,
              color: Colors.white,
            ),
          ),
        ),
      ),
    );
  }
}

class _HomeBody extends StatelessWidget {
  const _HomeBody({required this.controller});

  final HomeController controller;

  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: const BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.only(
          topRight: Radius.circular(30),
          bottomRight: Radius.circular(30),
        ),
        boxShadow: <BoxShadow>[
          BoxShadow(
            color: AppColor.theme,
            blurRadius: 1,
            offset: Offset(3, 0),
          ),
        ],
      ),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.spaceAround,
        children: [
          _buildTitle(),
          Container(
            alignment: Alignment.topCenter,
            child: _ModeToggle(controller: controller),
          ),
          Padding(
            padding: EdgeInsets.symmetric(horizontal: baseWidth * 0.075),
            child: Wrap(
              spacing: baseWidth * 0.05,
              runSpacing: baseHeight * 0.025,
              children:
                  controller.menu.map((menu) => _buildMenu(menu)).toList(),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildTitle() {
    return Column(
      children: [
        Text(
          "PTC",
          style: TextStyle(
            fontSize: baseWidth * 0.1,
            fontWeight: FontWeight.bold,
            color: AppColor.theme,
          ),
        ),
        const Text(
          "People Traffic Control",
          style: TextStyle(
            fontSize: 12,
            fontWeight: FontWeight.bold,
            color: AppColor.theme,
          ),
        ),
      ],
    );
  }

  Widget _buildMenu(Map<String, dynamic> menu) {
    return Column(
      children: <Widget>[
        _buildCircleButton(menu, baseWidth * 0.15),
        Text(
          menu['title'],
          textAlign: TextAlign.center,
          style: TextStyle(
            fontSize: baseWidth * 0.03,
            fontWeight: FontWeight.w600,
          ),
        ),
      ],
    );
  }

  Widget _buildCircleButton(Map menu, double size) {
    return GestureDetector(
      onTapCancel: () => controller.changeMenuState(menu),
      onTapDown: (_) => controller.changeMenuState(menu),
      onTapUp: (_) {
        controller.changeMenuState(menu);
        Get.toNamed(
          menu['route'],
          arguments: {
            'menu': menu['title'],
            'route': menu['route'],
            'mode': controller.mode['mode'],
            'color': controller.mode['color'],
          },
        )?.then((value) {
          controller.showPass("Success", value);
          return controller.getLogActivity();
        });
      },
      child: Obx(() {
        return AnimatedContainer(
          duration: const Duration(milliseconds: 100),
          width: menu['state'].value ? size * 0.85 : size,
          height: menu['state'].value ? size * 0.85 : size,
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            boxShadow: <BoxShadow>[
              if (!menu['state'].value)
                const BoxShadow(
                  color: AppColor.shadow,
                  spreadRadius: 2,
                  blurRadius: 3,
                ),
            ],
          ),
          child: Image.asset(menu['icon']),
        );
      }),
    );
  }
}

class _ModeToggle extends StatelessWidget {
  const _ModeToggle({required this.controller});

  final dynamic controller;

  @override
  Widget build(BuildContext context) {
    return Obx(() {
      return GestureDetector(
        onTap: () => controller.changeMode(),
        child: Container(
          width: baseWidth * 0.4,
          height: baseHeight * 0.05,
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(100),
            color: AppColor.off,
          ),
          child: Stack(
            children: <Widget>[
              Center(
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.spaceAround,
                  children: List.generate(
                    controller.modes.length,
                    (index) => Text(
                      controller.modes[index]['mode'].toUpperCase(),
                      style: TextStyle(
                        fontSize: baseHeight * 0.025,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  ),
                ),
              ),
              AnimatedAlign(
                duration: const Duration(milliseconds: 400),
                alignment: controller.mode['position'],
                curve: Curves.decelerate,
                child: Container(
                  width: baseWidth * 0.215,
                  alignment: Alignment.center,
                  decoration: BoxDecoration(
                    color: controller.mode['color'],
                    borderRadius: BorderRadius.circular(30),
                  ),
                  child: Text(
                    controller.mode['mode'].toUpperCase(),
                    style: TextStyle(
                      color: Colors.white,
                      fontSize: baseHeight * 0.025,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
      );
    });
  }
}
