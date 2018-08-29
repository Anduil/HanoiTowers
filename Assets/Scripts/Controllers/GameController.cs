using System;
using System.Linq;
using Common;
using Controllers;
using Controls;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    [RequireComponent(typeof(UIController))]
    public class GameController : MonoBehaviour
    {
        [SerializeField] private GameObject piePrefab;
        [SerializeField] private GameObject pieGroundPrefab;

        private readonly Tower[] towers = new Tower[3];
        private int piesCount;
        private int step = 0;
        private bool isPlaying = false;

        private MonoCache<PieControl> pieCache;
        private MonoCache<PieControl> pieGroundsCache;

        private UIController ui;
        private PieControl activePie;

        private void OnValidate()
        {
            if (piePrefab != null && piePrefab.GetComponent<PieControl>() == null)
            {
                piePrefab = null;
            }
            if (pieGroundPrefab != null && pieGroundPrefab.GetComponent<PieControl>() == null)
            {
                pieGroundPrefab = null;
            }
        }

        private void Awake()
        {
            ui = GetComponent<UIController>();
            pieCache = new MonoCache<PieControl>(piePrefab, null);
            pieGroundsCache = new MonoCache<PieControl>(pieGroundPrefab, null);
            ui.OnClick.AddListener(OnPlayButtonPressed);
        }

        private void Start()
        {
            ResetGame();
        }

        private void OnDestroy()
        {
            ui.OnClick.RemoveListener(OnPlayButtonPressed);
        }

        [UsedImplicitly]
        public void OnPlayButtonPressed()
        {
            if (isPlaying)
            {
                ResetGame();
            }
            else
            {
                var count = ui.GetCountPies();
                if (count > 0)
                {
                    ui.SetState(UIController.States.Playing);
                    Initial(count);
                    NextStep();
                    isPlaying = true;
                }
            }
        }

        private void Initial(int piesCount)
        {
            if(piesCount < 1)
                throw new ArgumentOutOfRangeException(nameof(piesCount));

            this.piesCount = piesCount;
            step = 0;

            // Pies grounds.
            var towersOffset = piesCount + 3;
            for (var i = -towersOffset; i <= towersOffset; i += towersOffset)
            {
                var control = pieGroundsCache.Spawn();
                control.transform.position = new Vector3(i, 0f, 0f);
                /*var ground = Instantiate(pieGroundPrefab, new Vector3(i, 0f, 0f), Quaternion.identity);
                var control = ground.GetComponent<PieControl>();*/
                control.Weight = piesCount + 1;
            }

            // Pie towers.
            towers[0] = new Tower { Position = new Vector3(-towersOffset, 1f, 0f) };
            towers[1] = new Tower { Position = new Vector3(0f, 1f, 0f) };
            towers[2] = new Tower { Position = new Vector3(towersOffset, 1f, 0f) };

            // Pies in first tower.
            for (var i = piesCount; i > 0; i--)
            {
                var control = pieCache.Spawn();
                control.transform.position = towers[0].ApexPosition;
                //var obj = Instantiate(piePrefab, towers[0].ApexPosition, Quaternion.identity);
                //var pieControl = obj.GetComponent<PieControl>();
                control.Weight = i;
                towers[0].Push(control);
            }
        }

        private void ResetGame()
        {
            isPlaying = false;
            ui.SetState(UIController.States.Stopped);
            pieCache.DespawnAll(p => p?.DisposeTween());
            pieGroundsCache.DespawnAll();
        }

        private void NextStep()
        {
            if (towers[1].Height >= piesCount || towers[2].Height >= piesCount)
            {
                ui.SetState(UIController.States.Finish);
            }
            else
            {
                if (step > 2)
                {
                    step = 0;
                }
                bool result;
                if (piesCount % 2 == 0)
                {
                    switch (step)
                    {
                        case 0:
                            result = Step(towers[0], towers[1]);
                            break;
                        case 1:
                            result = Step(towers[0], towers[2]);
                            break;
                        case 2:
                            result = Step(towers[1], towers[2]);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(step));
                    }
                }
                else
                {
                    switch (step)
                    {
                        case 0:
                            result = Step(towers[0], towers[2]);
                            break;
                        case 1:
                            result = Step(towers[0], towers[1]);
                            break;
                        case 2:
                            result = Step(towers[1], towers[2]);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(step));
                    }
                }
                step += 1;
                if (!result)
                {
                    throw new Exception();
                }
            }
        }

        private bool Step(Tower firstTower, Tower secondTower)
        {
            var first = firstTower.Peek();
            var second = secondTower.Peek();

            if (first == null && second == null)
                return false;

            if (first == null || (second != null && first.Weight > second.Weight))
            {
                second.Move(GetMaxHeight(), firstTower.ApexPosition, () =>
                {
                    var pie = secondTower.Pop();
                    firstTower.Push(pie);
                    NextStep();
                });
            }
            else
            {
                first.Move(GetMaxHeight(), secondTower.ApexPosition, () =>
                {
                    var pie = firstTower.Pop();
                    secondTower.Push(pie);
                    NextStep();
                });
            }

            return true;
        }

        private float GetMaxHeight()
        {
            return towers.Max(x => x.Height) + 3f;
        }
    }
}
